using System;
using TheTrial.Data.ScriptableData;
using UnityEditor.Experimental;
using UnityEngine;

namespace TheTrial.GameCore.Animations.Behaviours
{
    public class SetAnimatorFlag : StateMachineBehaviour
    {
        [NonSerialized] // 
        public AnimatorInfo animatorInfo;
        public ScriptableAnimatorFlag animatorFlag;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animatorInfo.Add(animatorFlag);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animatorInfo.Subtract(animatorFlag);
        }
    }
}
