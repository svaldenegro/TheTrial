using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TheTrial.Data.ScriptableData;
using TheTrial.GameCore.Animations.Behaviours;
using UnityEngine;

namespace TheTrial.GameCore.Animations
{
    public class AnimatorInfo : MonoBehaviour
    {
        public Animator animator;
        [ShowInInspector]
        private Dictionary<ScriptableAnimatorFlag, int> flags;
        
        private void OnEnable()
        {
            flags = new Dictionary<ScriptableAnimatorFlag, int>();
            var setAnimatorFlags = animator.GetBehaviours<SetAnimatorFlag>();
            foreach (var setAnimatorFlag in setAnimatorFlags)
            {
                setAnimatorFlag.animatorInfo = this;
            }
        }

        // Add method addition and subtract of flags, Add(ScriptableAnimatorFlag flag) adds +1 to the flag entry or add the entry in 1 if it doesn't exist, inverted for subtract
        public void Add(ScriptableAnimatorFlag flag)
        {
            if (!flags.TryAdd(flag, 1))
            {
                flags[flag]++;
            }
        }
        
        public void Subtract(ScriptableAnimatorFlag flag)
        {
            if (!flags.TryGetValue(flag, out var value)) return;
            if (value == 1)
            {
                flags.Remove(flag);
            }
            else
            {
                flags[flag] -= 1;
            }
        }
        
        public bool HasFlag(ScriptableAnimatorFlag flag)
        {
            if (flags.TryGetValue(flag, out var value))
            {
                return value > 0;
            }
            return false;
        }
    }
}