using Sirenix.OdinInspector;
using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Animations;
using TheTrial.GameCore.Controllers;
using UnityEngine;

namespace TheTrial.GameCore.Actions
{
    public class ChargeAction : MonoBehaviour
    {
        [SerializeField] //
        private ControllerBase controller;

        [SerializeField] //
        private HumanoidAnimator animator;

        [SerializeField] //
        private AnimatorInfo animatorInfo;

        [SerializeField] //
        private ScriptableAnimatorFlag movementFlag;

        [SerializeField] //
        private ScriptableAnimatorFlag chargeFlag;

        [SerializeField] //
        private float chargeForce = 1f;

        public bool CanCharge()
        {
            return controller.IsGrounded && animatorInfo.HasFlag(movementFlag);
        }

        public void Charge()
        {
            controller.extraSpeed = chargeForce;
            animator.Charge(true);

            if (controller.IsGrounded && animatorInfo.HasFlag(chargeFlag))
                return;
            
            Cancel();
        }

        public void Cancel()
        {
            controller.extraSpeed = 0;
            animator.Charge(false);
        }
    }
}