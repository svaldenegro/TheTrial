using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheTrial.GameCore.Animations
{
    public class IKController : MonoBehaviour
    {
        [Flags]
        public enum IKFlags
        {
            Position = 1,
            Rotation = 2,
            Both = Position | Rotation
        }
        
        private class IKData
        {
            public IKFlags flags;
            public Vector3 position;
            public Quaternion rotation;
            public float speed = 1;
            public float currentPositionWeight;
            public float currentRotationWeight;
            public float targetPositionWeight;
            public float targetRotationWeight;
        }

        public Animator animator;
        private Dictionary<AvatarIKGoal, IKData> _ikSet = new();

        private void Start()
        {
            _ikSet = new();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            foreach (var ikDatum in _ikSet)
            {
                var ikGoal = ikDatum.Key;
                var ikData = ikDatum.Value;
                
                animator.SetIKPosition(ikGoal, ikData.position);
                animator.SetIKRotation(ikGoal, ikData.rotation);
                var delta = ikData.speed * Time.deltaTime;
                animator.SetIKPositionWeight(ikGoal, ikData.currentPositionWeight = Mathf.MoveTowards(ikData.currentPositionWeight, ikData.targetPositionWeight, delta));
                animator.SetIKRotationWeight(ikGoal, ikData.currentRotationWeight = Mathf.MoveTowards(ikData.currentRotationWeight, ikData.targetRotationWeight, delta));
            }
        }
    }
}