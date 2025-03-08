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

        [Title("Components")] // 
        public ControllerBase controller;

        public JumpAction jumpAction;
        public RollAction rollAction;
        public ChargeAction chargeAction;
        public PrimaryAction primaryAction;
        public AimAction aimAction;
        public new GameplayCamera camera;

        [Title("Parameters"), SerializeField] private float cameraSpeed = 10;
        [SerializeField] private float characterRotationSpeed = 100f;
        [SerializeField] private float characterRotationSpeedRootMotion = 50f;
        [SerializeField] private float controllerModeHoldTime = 0.5f;

        [Title("Lock Focus Parameters"), SerializeField] private LayerMask enemyMask;
        [SerializeField] private float lockFocusDist = 10f;

        [Title("Hidden", "Private variables")] // 
        [ShowInInspector, ReadOnly]
        private ControllerMode _controllerMode = ControllerMode.Free;
        private InputAction _characterMovement, _cameraMovement, _jump, _roll, _charge, _primary, _secondary;
        private Quaternion _rotation;
        [ShowInInspector, ReadOnly] private Target _target;
        private float _secondaryHold;

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

        private void PrimaryAction(InputAction.CallbackContext context) => primaryAction.Activate();
        private void ChargeCanceled(InputAction.CallbackContext context) => chargeAction.Cancel();
        private void Charge(InputAction.CallbackContext context) => chargeAction.Charge();
        private void Roll(InputAction.CallbackContext context) => rollAction.Roll();
        private void Jump(InputAction.CallbackContext context) => jumpAction.Jump();

        private void UnlockController(ControllerMode mode)
        {
            _controllerMode = mode;
            camera.StopLookingAt();
        }

        private void Update()
        {
            MoveCamera();
            
            SecondaryActionManagement();
            MoveController();
        }

        private void SecondaryActionManagement()
        {
            if (_secondary.IsPressed())
            {
                _secondaryHold += Time.deltaTime;
                if (_secondaryHold < controllerModeHoldTime)
                    return;

                Debug.DrawRay(transform.position + transform.rotation * Vector3.one, camera.Rotation * Vector3.forward,
                    Color.red);
                UnlockController(aimAction.Aim(camera.Rotation) ? ControllerMode.Focusing : ControllerMode.Free);
            }
            else
            {
                if (_secondaryHold == 0)
                    return;

                if (_secondaryHold < controllerModeHoldTime)
                {
                    if (_target is not null)
                    {
                        _target = null;
                        UnlockController(ControllerMode.Free);
                    }
                    else if (Physics.Raycast(camera.Pivot, camera.Direction, out var hit, lockFocusDist, enemyMask))
                    {
                        if (hit.transform.TryGetComponent(out _target))
                            _controllerMode = ControllerMode.Locked;
                    }
                }
                else
                {
                    // Do stuff for long press
                    aimAction.Cancel();
                    UnlockController(ControllerMode.Free);
                }

                _secondaryHold = 0;
            }
        }

        private void MoveController()
        {
            switch (_controllerMode)
            {
                case ControllerMode.Free:
                    FreeMovement();
                    break;
                case ControllerMode.Focusing:
                    FocusedMovement();
                    break;
                case ControllerMode.Locked:
                    LockedMovement();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FocusedMovement()
        {
            var rotation = camera.YawRotation;
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
            _rotation = camera.YawRotation *
                        Quaternion.LookRotation(new Vector3(displacement.x, 0, displacement.y).normalized);
            controller.Movement = _rotation * new Vector3(0, 0, Mathf.Min(1, displacement.magnitude));
        }

        private void LockedMovement()
        {
            if (_target is null)
            {
                UnlockController(ControllerMode.Free);
                return;
            }

            var direction = (_target.transform.position - controller.transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            controller.Rotation = Quaternion.RotateTowards(controller.Rotation, targetRotation,
                characterRotationSpeed * Time.deltaTime);
            
            var displacement = _characterMovement.ReadValue<Vector2>();

            if (displacement.sqrMagnitude < 0.01f)
                return;
            
            controller.Movement = camera.YawRotation * new Vector3(displacement.x, 0, displacement.y).normalized;
        }

        private void MoveCamera()
        {
            // Moving camera first.
            if (_controllerMode == ControllerMode.Locked)
            {
                camera.LookAt = Quaternion.LookRotation((_target.Position - camera.Pivot).normalized);
                return;
            }
            camera.Move(_cameraMovement.ReadValue<Vector2>() * (cameraSpeed * Time.deltaTime));
        }
    }
}