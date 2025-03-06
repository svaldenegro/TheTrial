using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Animations;
using TheTrial.GameCore.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheTrial.GameCore.Actions
{
    public class RollAction : MonoBehaviour
    {
        [SerializeField]
        private ControllerBase controller;
        [SerializeField]
        private AnimatorController animator;
        [SerializeField]
        private ScriptableAnimatorFlag movementFlag;

        public void Roll()
        {
            if (!controller.IsGrounded || !animator.HasFlag(movementFlag)) return;
            
            animator.Roll();
        }
    }
}