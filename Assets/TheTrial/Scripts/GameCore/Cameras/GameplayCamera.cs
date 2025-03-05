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

        private float yaw;
        [ShowInInspector] //
        private float distance;

        public Quaternion Direction => Quaternion.Euler(0, yaw, 0);

        private void Start()
        {
            distance = offsetDistance;
        }

        private void LateUpdate()
        {
            var rotation = Direction;
            transform.rotation = rotation * Quaternion.Euler(pitch.Value, 0, 0);

            var pivot = rotation * pivotOffset + target.position;
            if (Physics.Raycast(pivot, -transform.forward, out var hit, offsetDistance, mask))
            {
                transform.position = hit.point;
                distance = hit.distance;
            }
            else
            {
                distance = Mathf.MoveTowards(distance, offsetDistance, recoverySpeed * Time.deltaTime);
                transform.position = pivot - transform.forward * distance;
            }
        }

        public void Move(Vector2 movement)
        {
            yaw += movement.x;
            pitch.Value -= movement.y;
        }
    }
}