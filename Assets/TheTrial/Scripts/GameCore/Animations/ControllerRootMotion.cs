using System;
using Sirenix.OdinInspector;
using TheTrial.GameCore.Animations.Behaviours;
using TheTrial.GameCore.Controllers;
using UnityEngine;

namespace TheTrial.GameCore.Animations
{
    public class ControllerRootMotion : MonoBehaviour
    {
        [Title("Components")]
        [SerializeField] //
        private ControllerBase controller;
        [SerializeField] //
        private Animator animator;
        [SerializeField] //
        private float rootMotionForce = 1f;
        public bool ApplyRootMotion
        {
            get => enabled;
            set => enabled = value;
        }

        private void Start()
        {
            var useRootMotions = animator.GetBehaviours<UseRootMotion>();

            foreach (var useRootMotion in useRootMotions)
            {
                useRootMotion.controllerRootMotion = this;
            }
            
            enabled = false;
        }

        private void OnAnimatorMove()
        {
            controller.Displacement = animator.deltaPosition * rootMotionForce;
        }
        
        // Auto assign OnValidate
        private void OnValidate()
        {
            animator ??= GetComponent<Animator>();
        }
    }
}
