using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheTrial.GameCore.Animations.Behaviours
{
    public class UseRootMotion : StateMachineBehaviour
    {
        [NonSerialized]
        public ControllerRootMotion controllerRootMotion;
        public bool value;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            controllerRootMotion.ApplyRootMotion = value;
        }
    }
}