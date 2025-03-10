using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
            public Vector3 position;
            public Quaternion rotation;
            public float currentPositionWeight;
            public float currentRotationWeight;
            public float targetPositionWeight;
            public float targetRotationWeight;
            public float speed = 1;
        }

        // head look at data
        private class LookAtData
        {
            public Vector3 position;
            public float currentWeight;
            public float targetWeight;
            public float speed = 1;
        }

        public Animator animator;

        [ShowInInspector, ReadOnly] private Dictionary<AvatarIKGoal, IKData> _ikSet = new();
        private LookAtData _lookAtData;

        private void Start()
        {
            _ikSet = new Dictionary<AvatarIKGoal, IKData>();
            _lookAtData = new LookAtData();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            foreach (var ikDatum in _ikSet)
            {
                var ikData = ikDatum.Value;
                var ikGoal = ikDatum.Key;
                if (ikData.currentPositionWeight == 0 && ikData.targetPositionWeight == 0 &&
                    ikData.currentRotationWeight == 0 && ikData.targetRotationWeight == 0)
                {
                    animator.SetIKPositionWeight(ikGoal, 0);
                    animator.SetIKRotationWeight(ikGoal, 0);
                    continue;
                }

                animator.SetIKPosition(ikGoal, ikData.position);
                animator.SetIKRotation(ikGoal, ikData.rotation);
                var delta = ikData.speed * Time.deltaTime;
                animator.SetIKPositionWeight(ikGoal,
                    ikData.currentPositionWeight = Mathf.MoveTowards(ikData.currentPositionWeight,
                        ikData.targetPositionWeight, delta));
                animator.SetIKRotationWeight(ikGoal,
                    ikData.currentRotationWeight = Mathf.MoveTowards(ikData.currentRotationWeight,
                        ikData.targetRotationWeight, delta));
            }

            // Update Look At Data
            if (_lookAtData.currentWeight == 0)
            {
                animator.SetLookAtWeight(0);
                return;
            }

            animator.SetLookAtPosition(_lookAtData.position);
            // Blend the look at weight
            _lookAtData.currentWeight = Mathf.MoveTowards(_lookAtData.currentWeight, _lookAtData.targetWeight,
                _lookAtData.speed * Time.deltaTime);
            animator.SetLookAtWeight(_lookAtData.currentWeight);
        }

        public void SetIK(AvatarIKGoal goal, Vector3 position, Quaternion rotation, float positionWeight,
            float rotationWeight, float speed = 1)
        {
            if (!_ikSet.TryGetValue(goal, out var ikData))
            {
                ikData = new IKData();
                _ikSet.Add(goal, ikData);
            }

            ikData.position = position;
            ikData.rotation = rotation;
            ikData.speed = speed;
            ikData.targetPositionWeight = positionWeight;
            ikData.targetRotationWeight = rotationWeight;
        }

        public void CancelIK(AvatarIKGoal goal)
        {
            if (!_ikSet.TryGetValue(goal, out var ikData)) return;
            ikData.targetPositionWeight = 0;
            ikData.targetRotationWeight = 0;
        }
        
        public void SetLookAt(Vector3 position, float weight, float speed = 1)
        {
            _lookAtData.position = position;
            _lookAtData.targetWeight = weight;
            _lookAtData.speed = speed;
        }
        
        public void CancelLookAt()
        {
            _lookAtData.targetWeight = 0;
        }
        
        // OnValidate try to get the animator
        private void OnValidate()
        {
            animator ??= GetComponent<Animator>();
        }
    }
}