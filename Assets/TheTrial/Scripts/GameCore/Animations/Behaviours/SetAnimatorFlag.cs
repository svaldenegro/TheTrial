using System;
using TheTrial.Data.ScriptableData;
using UnityEditor.Experimental;
using UnityEngine;

namespace TheTrial.GameCore.Animations.Behaviours
{
    public class SetAnimatorFlag : StateMachineBehaviour
    {
        [NonSerialized] // 
        public AnimatorController animator;
        public ScriptableAnimatorFlag animatorFlag;

        public override void OnStateEnter(Animator _, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.Add(animatorFlag);
        }

        public override void OnStateExit(Animator _, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.Subtract(animatorFlag);
        }
    }
}
