using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Animations;
using TheTrial.GameCore.Controllers;
using UnityEngine;

namespace TheTrial.GameCore.Actions
{
    public class RollAction : MonoBehaviour
    {
        [SerializeField]
        private ControllerBase controller;
        [SerializeField]
        private HumanoidAnimator animator;
        [SerializeField]
        private AnimatorInfo animatorInfo;
        [SerializeField]
        private ScriptableAnimatorFlag movementFlag;

        public void Roll()
        {
            if (!controller.IsGrounded || !animatorInfo.HasFlag(movementFlag)) return;
            
            animator.Roll();
        }
    }
}