using System.Collections.Generic;
using Sirenix.OdinInspector;
using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Actions;
using TheTrial.GameCore.Animations.Behaviours;
using TheTrial.GameCore.Controllers;
using UnityEngine;

namespace TheTrial.GameCore.Animations
{
    public class AnimatorController : MonoBehaviour
    {
        private static readonly int StrafeHash = Animator.StringToHash("Strafe");
        private static readonly int ForwardHash = Animator.StringToHash("Forward");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int JumpHash = Animator.StringToHash("Jump");
        private static readonly int RollHash = Animator.StringToHash("Roll");
        private static readonly int ChargingHash = Animator.StringToHash("Charging");
        private static readonly int PrimaryActionHash = Animator.StringToHash("PrimaryAction");
        private static readonly int FocusingHash = Animator.StringToHash("Focusing");
        

        [Title("Components")] //
        public Animator animator;
        public ControllerBase controller;

        [Title("Parameters")] // 
        public float speedSoftener = 1f;

        private Vector3 _lastPosition;
        private float _strafe, _forward;
        
        [ShowInInspector]
        private Dictionary<ScriptableAnimatorFlag, int> _flags;
        
        private void OnEnable()
        {
            _flags = new Dictionary<ScriptableAnimatorFlag, int>();
            var setAnimatorFlags = animator.GetBehaviours<SetAnimatorFlag>();
            foreach (var setAnimatorFlag in setAnimatorFlags)
            {
                setAnimatorFlag.animator = this;
            }
        }
        
        private void LateUpdate()
        {
            animator.SetBool(IsGroundedHash, controller.IsGrounded);
            
            var velocity = (transform.position - _lastPosition) / Time.deltaTime / controller.Speed;
            
            _strafe = Mathf.MoveTowards(_strafe, Vector3.Dot(transform.right, velocity), Time.deltaTime * speedSoftener);
            animator.SetFloat(StrafeHash, _strafe);
            _forward = Mathf.MoveTowards(_forward, Vector3.Dot(transform.forward, velocity), Time.deltaTime * speedSoftener);
            animator.SetFloat(ForwardHash, _forward);
            
            _lastPosition = transform.position;
        }

        public void Jump() => animator.SetTrigger(JumpHash);
        public void Roll() => animator.SetTrigger(RollHash);
        public void PrimaryAction() => animator.SetTrigger(PrimaryActionHash);

        public void Charge(bool value) => animator.SetBool(ChargingHash, value);
        public void Focused(bool value) => animator.SetBool(FocusingHash, value);

        // Add method addition and subtract of flags, Add(ScriptableAnimatorFlag flag) adds +1 to the flag entry or add the entry in 1 if it doesn't exist, inverted for subtract
        public void Add(ScriptableAnimatorFlag flag)
        {
            if (!_flags.TryAdd(flag, 1))
            {
                _flags[flag]++;
            }
        }
        
        public void Subtract(ScriptableAnimatorFlag flag)
        {
            if (!_flags.TryGetValue(flag, out var value)) return;
            if (value == 1)
            {
                _flags.Remove(flag);
            }
            else
            {
                _flags[flag] -= 1;
            }
        }
        
        public bool HasFlag(ScriptableAnimatorFlag flag)
        {
            if (_flags.TryGetValue(flag, out var value))
            {
                return value > 0;
            }
            return false;
        }
        
        // OnValidate try to get the animator
        private void OnValidate()
        {
            animator ??= GetComponent<Animator>();
        }
    }
}
