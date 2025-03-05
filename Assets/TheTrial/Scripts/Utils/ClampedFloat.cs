using System;
using UnityEngine;

namespace TheTrial.Scripts.Utils
{
    [Serializable]
    public struct ClampedFloat
    {
        public float min, max;
        private float value;

        public float Value
        {
            get => value;
            set => this.value = Mathf.Clamp(value, min, max);
        }
    }
}