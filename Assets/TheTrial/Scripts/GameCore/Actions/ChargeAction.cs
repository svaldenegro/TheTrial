using Sirenix.OdinInspector;
using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Animations;
using TheTrial.GameCore.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheTrial.GameCore.Actions
{
    public class ChargeAction : MonoBehaviour
    {
        [SerializeField] //
        private ControllerBase controller;

        [SerializeField] //
        private AnimatorController animator;

        [SerializeField] //
        private ScriptableAnimatorFlag movementFlag;

        [SerializeField] //
        private ScriptableAnimatorFlag chargeFlag;

        [SerializeField] //
        private float chargeForce = 1f;

        public bool CanCharge()
        {
            return controller.IsGrounded && animator.HasFlag(movementFlag);
        }

        public void Charge()
        {
            if (controller.IsGrounded && (animator.HasFlag(movementFlag) || animator.HasFlag(chargeFlag)))
            {
                controller.extraSpeed = chargeForce;
                animator.Charge(true);
                return;
            }
            
            Cancel();
        }

        public void Cancel()
        {
            controller.extraSpeed = 0;
            animator.Charge(false);
        }
    }
}