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

        private Vector3 displacement, movement, gravity;

        public override Vector3 Movement
        {
            set => movement += value;
        }

        public override Vector3 Displacement
        {
            set => displacement += value;
        }

        public override Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public override bool IsGrounded => controller.isGrounded;
        
        public override void Jump(float force)
        {
            gravity = -gravityForce * force;
        }

        private void Update()
        {
            controller.Move((displacement.sqrMagnitude < 0.0001f ? movement * (Speed * Time.deltaTime) : displacement) + gravity);
            if (controller.isGrounded)
                gravity = gravityForce * gravityMagnetForce;
            else
                gravity += gravityForce * (gravityMultiplier * Time.deltaTime);

            displacement = Vector3.zero;
            movement = Vector3.zero;
        }

        private void OnValidate()
        {
            controller ??= GetComponent<CapsuleController>();
        }
        
        // Print gravity force in the editor
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 400, 40), displacement.ToString());
            GUI.Label(new Rect(10, 50, 400, 40), displacement.sqrMagnitude.ToString(CultureInfo.InvariantCulture));
        }
    }
}