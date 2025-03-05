using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheTrial.Data.ScriptableData
{
    [CreateAssetMenu(menuName = Paths.Data + nameof(ScriptablePhysicLayer), fileName = nameof(ScriptablePhysicLayer), order = 0)]
    public class ScriptablePhysicLayer : ScriptableObject
    {
        [ValueDropdown(nameof(GetPhysicsLayersDropdown))]
        public int id;

        public IEnumerable GetPhysicsLayersDropdown()
        {
            var list = new ValueDropdownList<int>();
            for (var i = 0; i < 32; i++)
            {
                var layer = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layer))
                    list.Add(layer, i);
            }
            return list;
        }
        
        public static implicit operator int(ScriptablePhysicLayer scriptablePhysicLayer)
        {
            return scriptablePhysicLayer.id;
        }
    }
}