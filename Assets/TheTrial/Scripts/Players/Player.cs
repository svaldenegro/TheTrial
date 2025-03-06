using System;
using Sirenix.OdinInspector;
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
        private enum ControllerMode
        {
            Free,
            Focusing,
            Locked
        }

        [Title("Components")] public ControllerBase controller;
        public JumpAction jumpAction;
        public RollAction rollAction;
        public ChargeAction chargeAction;
        public PrimaryAction primaryAction;
        public AimAction aimAction;
        public new GameplayCamera camera;

        [Title("Parameters")]
        public float cameraSpeed = 10, characterRotationSpeed = 100f, characterRotationSpeedRootMotion = 50f;

        public float controllerModeHoldTime = 0.5f;

        [Title("Hidden", "Private variables")] [ShowInInspector, ReadOnly]
        private ControllerMode _controllerMode = ControllerMode.Free;

        private InputAction _characterMovement, _cameraMovement, _jump, _roll, _charge, _primary, _secondary;
        private Quaternion _rotation;

        private void Start()
        {
            _characterMovement = InputSystem.actions.FindAction("Move");
            _cameraMovement = InputSystem.actions.FindAction("Look");
            _jump = InputSystem.actions.FindAction("Jump");
            if (_jump is not null)
                _jump.canceled += Jump;
            _roll = InputSystem.actions.FindAction("Roll");
            if (rollAction is not null)
                _roll.canceled += Roll;
            _primary = InputSystem.actions.FindAction("Primary");
            if (primaryAction is not null)
            {
                _primary.canceled += PrimaryAction;
            }

            _secondary = InputSystem.actions.FindAction("Secondary");
            _secondary.performed += SecondaryActionHold;
            _secondary.canceled += SecondaryActionRelease;
            _charge = InputSystem.actions.FindAction("Charge");
            if (chargeAction is not null)
            {
                _charge.performed += Charge;
                _charge.canceled += ChargeCanceled;
            }

            // Lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SecondaryActionHold(InputAction.CallbackContext context)
        {
            if (context.time < controllerModeHoldTime)
                return;
            if (controller.overrideMovement)
            {
                _controllerMode = ControllerMode.Free;
                aimAction.CancelAim();
                return;
            }

            _controllerMode = ControllerMode.Focusing;
            aimAction.Aim(camera.Direction);
        }

        private void SecondaryActionRelease(InputAction.CallbackContext context)
        {
            aimAction.CancelAim();
            if (context.time > controllerModeHoldTime)
            {
                _controllerMode = ControllerMode.Free;
                return;
            }
            _controllerMode = ControllerMode.Free;
        }

        private void PrimaryAction(InputAction.CallbackContext context)
        {
            primaryAction.Activate();
        }

        private void ChargeCanceled(InputAction.CallbackContext context)
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

            switch (_controllerMode)
            {
                case ControllerMode.Free:
                    FreeMovement();
                    break;
                case ControllerMode.Focusing:
                    FocusedMovement();
                    break;
                case ControllerMode.Locked:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FocusedMovement()
        {
            var rotation = camera.Direction;
            controller.Rotation = rotation;
            var displacement = _characterMovement.ReadValue<Vector2>();
            if (displacement.sqrMagnitude < 0.01f)
                return;
            controller.Movement = rotation * new Vector3(displacement.x, 0, displacement.y).normalized * 0.5f;
        }

        private void FreeMovement()
        {
            var displacement = _characterMovement.ReadValue<Vector2>();

            if (displacement.sqrMagnitude < 0.01f)
                return;

            controller.Rotation = Quaternion.RotateTowards(controller.Rotation, _rotation,
                (controller.overrideMovement ? characterRotationSpeedRootMotion : characterRotationSpeed) *
                Time.deltaTime);
            _rotation = camera.Direction *
                        Quaternion.LookRotation(new Vector3(displacement.x, 0, displacement.y).normalized);
            controller.Movement = _rotation * new Vector3(0, 0, Mathf.Min(1, displacement.magnitude));
        }

        private void MoveCamera()
        {
            // Moving camera first.
            camera.Move(_cameraMovement.ReadValue<Vector2>() * (cameraSpeed * Time.deltaTime));
        }
    }
}