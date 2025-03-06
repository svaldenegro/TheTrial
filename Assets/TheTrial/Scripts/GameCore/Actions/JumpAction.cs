using System;
using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Animations;
using TheTrial.GameCore.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheTrial.GameCore.Actions
{
    public class JumpAction : MonoBehaviour
    {
        [SerializeField] //
        private ControllerBase controller;
        [SerializeField] // 
        private AnimatorController animator;
        [SerializeField]
        private ScriptableAnimatorFlag movementFlag;
        [SerializeField] //
        private float jumpForce = 10;
        
        public void Jump()
        {
            if (!controller.IsGrounded || !animator.HasFlag(movementFlag)) return;
            controller.Jump(jumpForce);
            animator.Jump();
        }
    }
}