using System;
using Sirenix.OdinInspector;
using TheTrial.Data.ScriptableData;
using TheTrial.Scripts.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheTrial.GameCore.Cameras
{
    public class GameplayCamera : MonoBehaviour
    {
        public Transform target;

        [SerializeField] //
        private Vector3 pivotOffset;
        [SerializeField] //
        private float offsetDistance;
        [SerializeField] //
        private ClampedFloat pitch;
        [SerializeField] //
        private LayerMask mask;

        [FormerlySerializedAs("recoverSpeed")] [SerializeField] //
        private float recoverySpeed = 1f;

        private float _yaw;
        private float _distance;
        private Vector3 _pivot;

        private Quaternion _lookAt;
        private bool _isLookingAt;

        public Quaternion YawRotation
        {
            get
            {
                if (_isLookingAt)
                {
                    
                }
                return Quaternion.Euler(0, _yaw, 0);
            }
        }

        public Quaternion Rotation => transform.rotation;
        public Vector3 Direction => transform.forward;
        public Vector3 Pivot => _pivot;

        public Quaternion LookAt
        {
            set
            {
                _lookAt = value;
                _isLookingAt = true;
            }
        }

        private void Start()
        {
            _distance = offsetDistance;
        }

        private void LateUpdate()
        {
            var rotation = YawRotation;

            if (_isLookingAt)
            {
                transform.rotation = _lookAt;
                _isLookingAt = false;
            }
            else
                transform.rotation = rotation * Quaternion.Euler(pitch.Value, 0, 0);

            _pivot = rotation * pivotOffset + target.position;
            if (Physics.Raycast(_pivot, -transform.forward, out var hit, offsetDistance, mask))
            {
                transform.position = hit.point;
                _distance = hit.distance;
            }
            else
            {
                _distance = Mathf.MoveTowards(_distance, offsetDistance, recoverySpeed * Time.deltaTime);
                transform.position = _pivot - transform.forward * _distance;
            }
        }

        public void Move(Vector2 movement)
        {
            _yaw += movement.x;
            pitch.Value -= movement.y;
        }

        public void StopLookingAt()
        {
            _isLookingAt = false;
            var euler = transform.eulerAngles;
            _yaw = euler.y;
            pitch.Value = euler.x;
        }
    }
}