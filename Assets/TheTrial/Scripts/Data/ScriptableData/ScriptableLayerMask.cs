using System;
using UnityEngine;

namespace TheTrial.Data.ScriptableData
{
    [CreateAssetMenu(menuName = Paths.Data + nameof(ScriptableLayerMask), fileName = nameof(ScriptableLayerMask), order = 0)]
    public class ScriptableLayerMask : ScriptableObject
    {
        [SerializeField]
        private LayerMask value;
        public event Action OnValueChanged;
        
        public LayerMask Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value == value) 
                    return;
                this.value = value;
                OnValueChanged?.Invoke();
            }
        }
        
        // override operator int
        public static implicit operator int(ScriptableLayerMask layerMask)
        {
            return layerMask.Value;
        }
    }
}