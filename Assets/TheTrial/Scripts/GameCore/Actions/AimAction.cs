using System;
using Sirenix.OdinInspector;
using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheTrial.GameCore.Actions
{
    public class AimAction : MonoBehaviour
    {
        [SerializeField] //
        private Animator animator;
        [SerializeField] //
        private AnimatorController animatorController;
        [SerializeField] //
        private IKController ikController;
        [SerializeField] //
        private ScriptableAnimatorFlag movement;
        [SerializeField] //
        private ScriptableAnimatorFlag focusing;
        [SerializeField, OnValueChanged(nameof(OnAimEulerOffsetChanged))] //
        private Vector3 aimEulerOffset;
        [SerializeField] //
        private Vector3 aimPositionOffset;
        [SerializeField] //
        private float loadSpeed = 0.25f;

        [SerializeField] //
        private bool debug;
        [SerializeField] //
        private Vector3 debugEuler;

        [ShowInInspector, ReadOnly]
        private Transform _shoulder, _neck;
        [ShowInInspector, ReadOnly]
        private Quaternion _aimRotOffset;

        private void Start()
        {
            _shoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            _neck = animator.GetBoneTransform(HumanBodyBones.Neck);
            _aimRotOffset = Quaternion.Euler(aimEulerOffset);
        }

        // private void Update()
        // {
        //     if (debug)
        //     {
        //         Aim(Quaternion.Euler(debugEuler));
        //     }
        // }

        public bool Aim(Quaternion direction)
        {
            var aiming = animatorController.HasFlag(movement) || animatorController.HasFlag(focusing);
            animatorController.Focused(aiming);
            if (aiming)
            {
                ikController.SetIK(AvatarIKGoal.RightHand, _shoulder.position + direction * aimPositionOffset,
                    direction * _aimRotOffset, 1, 1, loadSpeed);
                ikController.SetLookAt(_neck.position + direction * Vector3.forward, 1, loadSpeed);
                return true;
            }

            ikController.SetIK(AvatarIKGoal.RightHand, _shoulder.position + direction * aimPositionOffset,
                direction * _aimRotOffset, 0, 0, loadSpeed);
            ikController.SetLookAt(_neck.position + direction * Vector3.forward, 0, loadSpeed);
            return false;
        }

        public void Cancel()
        {
            animatorController.Focused(false);
            ikController.CancelIK(AvatarIKGoal.RightHand);
            ikController.CancelLookAt();
        }

        private void OnAimEulerOffsetChanged()
        {
            _aimRotOffset = Quaternion.Euler(aimEulerOffset);
        }
    }
}