using System;
using UnityEngine;

namespace TheTrial.Players
{
    public class Target : MonoBehaviour
    {
        public Vector3 viewOffset;
        public Vector3 Position => transform.position + transform.rotation * viewOffset;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.rotation * viewOffset, 0.1f);
        }
    }
}
