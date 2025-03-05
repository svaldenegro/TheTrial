using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheTrial.GameCore.Controllers
{
    public abstract class ControllerBase : MonoBehaviour
    {
        [Title("Base Parameters")]
        [SerializeField] // 
        protected float baseSpeed;
        [ShowInInspector, ReadOnly, NonSerialized] //
        public float extraSpeed;
        
        public abstract Vector3 Movement { set; }
        public abstract Vector3 Displacement { set; }
        public abstract Quaternion Rotation { get; set; }

        public float Speed => baseSpeed + extraSpeed;

        public abstract bool IsGrounded { get; }
        public abstract void Jump(float force);
    }
}