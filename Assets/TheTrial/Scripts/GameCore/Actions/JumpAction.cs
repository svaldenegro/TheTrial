using System;
using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Animations;
using TheTrial.GameCore.Controllers;
using UnityEngine;

namespace TheTrial.GameCore.Actions
{
    public class JumpAction : MonoBehaviour
    {
        [SerializeField] //
        private ControllerBase controller;
        [SerializeField] 
        private HumanoidAnimator animator;
        [SerializeField]
        private AnimatorInfo animatorInfo;
        [SerializeField]
        private ScriptableAnimatorFlag movementFlag;
        [SerializeField] //
        private float jumpForce = 10;
        
        public void Jump()
        {
            if (!controller.IsGrounded || !animatorInfo.HasFlag(movementFlag)) return;
            controller.Jump(jumpForce);
            animator.Jump();
        }
    }
}