using Sirenix.OdinInspector;
using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheTrial.GameCore.Actions
{
    public class PrimaryAction : MonoBehaviour
    {
        [SerializeField] //
        private AnimatorController animator;
        [SerializeField] //
        private ScriptableAnimatorFlag movementFlag;

        public void Activate()
        {
            animator.PrimaryAction();
        }
    }
}
