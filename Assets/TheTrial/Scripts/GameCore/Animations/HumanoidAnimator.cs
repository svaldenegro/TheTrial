using Sirenix.OdinInspector;
using TheTrial.GameCore.Actions;
using TheTrial.GameCore.Controllers;
using UnityEngine;

namespace TheTrial.GameCore.Animations
{
    public class HumanoidAnimator : MonoBehaviour
    {
        private static readonly int StrafeHash = Animator.StringToHash("Strafe");
        private static readonly int ForwardHash = Animator.StringToHash("Forward");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int JumpHash = Animator.StringToHash("Jump");
        private static readonly int RollHash = Animator.StringToHash("Roll");
        private static readonly int ChargeHash = Animator.StringToHash("Charge");

        [Title("Components")] //
        public Animator animator;
        public ControllerBase controller;

        [Title("Parameters")] // 
        public float speedSoftener = 1f;

        private Vector3 lastPosition;
        private float strafe, forward;
        
        private void LateUpdate()
        {
            animator.SetBool(IsGroundedHash, controller.IsGrounded);
            
            var velocity = (transform.position - lastPosition) / Time.deltaTime / controller.Speed;
            
            strafe = Mathf.MoveTowards(strafe, Vector3.Dot(transform.right, velocity), Time.deltaTime * speedSoftener);
            animator.SetFloat(StrafeHash, strafe);
            forward = Mathf.MoveTowards(forward, Vector3.Dot(transform.forward, velocity), Time.deltaTime * speedSoftener);
            animator.SetFloat(ForwardHash, forward);
            
            lastPosition = transform.position;
        }

        public void Jump() => animator.SetTrigger(JumpHash);
        public void Roll() => animator.SetTrigger(RollHash);

        public void Charge(bool value)
        {
            animator.SetBool(ChargeHash, value);
        }
    }
}
