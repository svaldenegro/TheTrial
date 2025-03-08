using UnityEngine;

namespace TheTrial.GameCore.Animations
{
    public abstract class RootMotionRouter : MonoBehaviour
    {
        public abstract void ApplyRootMotion(Vector3 delta);
    }
}
