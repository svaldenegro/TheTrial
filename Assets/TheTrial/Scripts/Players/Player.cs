using System;
using TheTrial.GameCore.Actions;
using TheTrial.GameCore.Cameras;
using TheTrial.GameCore.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace TheTrial.Players
{
    public class Player : MonoBehaviour
    {
        public ControllerBase controller;
        public JumpAction jumpAction;
        public RollAction rollAction;
        public ChargeAction chargeAction;
        public new GameplayCamera camera;

        public float cameraSpeed = 10, characterRotationSpeed = 100f;

        private InputAction characterMovement, cameraMovement, jump, roll, charge, primary, secondary;
        private Quaternion rotation;

        private void Start()
        {
            characterMovement = InputSystem.actions.FindAction("Move");
            cameraMovement = InputSystem.actions.FindAction("Look");
            jump = InputSystem.actions.FindAction("Jump");
            if (jump is not null)
                jump.canceled += Jump;
            roll = InputSystem.actions.FindAction("Roll");
            if (rollAction is not null)
                roll.canceled += Roll;
            primary = InputSystem.actions.FindAction("Primary");
            secondary = InputSystem.actions.FindAction("Secondary");
            charge = InputSystem.actions.FindAction("Charge");
            if (chargeAction is not null)
            {
                charge.performed += Charge;
                charge.canceled += ChargeCanceled;
            }

            // Lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void ChargeCanceled(InputAction.CallbackContext obj)
        {
            chargeAction.Cancel();
        }

        private void Charge(InputAction.CallbackContext context)
        {
            chargeAction.Charge();
        }

        private void Roll(InputAction.CallbackContext context) => rollAction.Roll();
        private void Jump(InputAction.CallbackContext context) => jumpAction.Jump();

        private void Update()
        {
            MoveCamera();
            MoveController();
        }

        private void MoveController()
        {
            // Moving controller.
            var displacement = characterMovement.ReadValue<Vector2>();
            
            controller.Rotation = Quaternion.RotateTowards(controller.Rotation, rotation, characterRotationSpeed * Time.deltaTime);
            
            if (!(displacement.sqrMagnitude > 0.01f))
                return;
            
            rotation = camera.Direction * Quaternion.LookRotation(new Vector3(displacement.x, 0, displacement.y).normalized);
            controller.Movement = rotation * new Vector3(0, 0, Mathf.Min(1, displacement.magnitude));
        }

        private void MoveCamera()
        {
            // Moving camera first.
            camera.Move(cameraMovement.ReadValue<Vector2>() * (cameraSpeed * Time.deltaTime));
        }
    }
}