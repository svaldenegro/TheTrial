using System;
using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;
using CapsuleController = UnityEngine.CharacterController;

namespace TheTrial.GameCore.Controllers
{
    public class PlayerController : ControllerBase
    {
        [Title("Components")]
        [SerializeField] //
        private CapsuleController controller;
        
        [Title("Parameters")]
        [SerializeField] //
        private Vector3 gravityForce = new(0, -9.8f, 0);
        /// <summary> Minimal gravity force to magnet the controller in the ground. </summary>
        [SerializeField] //
        private float gravityMultiplier = 0.25f;
        [SerializeField] //
        private float gravityMagnetForce = 0.01f;

        private Vector3 _gravity;

        public override Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public override bool IsGrounded => controller.isGrounded;
        
        public override void Jump(float force)
        {
            _gravity = -gravityForce * force;
        }

        protected override void Move(Vector3 displacement, float deltaTime)
        {
            controller.Move(displacement + _gravity);
            
            if (controller.isGrounded)
                _gravity = gravityForce * gravityMagnetForce;
            else
                _gravity += gravityForce * (gravityMultiplier * deltaTime);
        }

        private void OnValidate()
        {
            controller ??= GetComponent<CapsuleController>();
        }
        
        // Print gravity force in the editor
        private void OnGUI()
        {
            // GUI.Label(new Rect(10, 10, 400, 40), _displacement.ToString());
            // GUI.Label(new Rect(10, 50, 400, 40), _displacement.sqrMagnitude.ToString(CultureInfo.InvariantCulture));
        }
    }
}