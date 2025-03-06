using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheTrial.GameCore.Controllers
{
    public abstract class ControllerBase : MonoBehaviour
    {
        public enum MovementType
        {
            Movement, Override
        }
        
        [Title("Base Parameters")]
        [SerializeField] // 
        protected float baseSpeed;
        [ShowInInspector, ReadOnly, NonSerialized] //
        public float extraSpeed;
        [ShowInInspector, ReadOnly, NonSerialized] //
        public bool overrideMovement;
        
        private Vector3 _movement, _movementOverride;

        public Vector3 Movement
        {
            set => _movement = value;
        }

        public Vector3 MovementOverride
        {
            set => _movementOverride = value;
        }

        public abstract Quaternion Rotation { get; set; }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            if (overrideMovement)
            {
                Move(_movementOverride, deltaTime);
                _movementOverride = Vector3.zero;
            }
            else
            {
                Move(_movement * (Speed * deltaTime), deltaTime);
                _movement = Vector3.zero;
            }
        }
        
        protected abstract void Move(Vector3 displacement, float deltaTime);

        public float Speed => baseSpeed + extraSpeed;

        public abstract bool IsGrounded { get; }
        public abstract void Jump(float force);
    }
}