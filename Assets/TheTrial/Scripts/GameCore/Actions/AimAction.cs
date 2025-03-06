using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Animations;
using UnityEngine;

namespace TheTrial.GameCore.Actions
{
    public class AimAction : MonoBehaviour
    {
        [SerializeField] //
        public AnimatorController animator;

        [SerializeField] //
        public ScriptableAnimatorFlag movement, focusing;

        public void Aim(Quaternion direction)
        {
            var aiming = (animator.HasFlag(movement) || animator.HasFlag(focusing));
            animator.Focused(aiming);
            if (aiming) 
            {
                animator.Aim(direction);
            }
        }

        public void CancelAim()
        {
            animator.Focused(false);
        }
    }
}