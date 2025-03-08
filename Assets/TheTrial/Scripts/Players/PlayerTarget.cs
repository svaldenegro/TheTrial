using System;
using UnityEngine;

namespace TheTrial.Players
{
    public class PlayerTarget : MonoBehaviour
    {
        public Vector3 viewOffset;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.rotation * viewOffset, 0.1f);
        }
    }
}
