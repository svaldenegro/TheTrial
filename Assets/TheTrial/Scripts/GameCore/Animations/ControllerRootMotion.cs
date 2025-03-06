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
            set => controller.overrideMovement = enabled = value;
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
            controller.MovementOverride = animator.deltaPosition * rootMotionForce;
            controller.transform.rotation *= animator.deltaRotation;

            // Opcional: Resetear la posición local del hijo para evitar doble transformación
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        
        // Auto assign OnValidate
        private void OnValidate()
        {
            animator ??= GetComponent<Animator>();
        }
    }
}
