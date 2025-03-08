using UnityEngine;
using UnityEngine.AI;

namespace TheTrial.Players
{
    public class BasicGhoul : MonoBehaviour
    {
        private CharacterController _controller;
        public Transform target;
        public float speed = 3.0f;
        public float rotationSpeed = 5.0f;
        public float stoppingDistance = 1.0f;

        private NavMeshPath _navPath;
        private int _currentCornerIndex;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _navPath = new NavMeshPath();

            if (target is not null) return;
            
            Debug.LogError("Target no asignado en el inspector!");
            enabled = false;
        }

        private void Update()
        {
            if (target is null) return;

            // Calcular nuevo camino si es necesario
            if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, _navPath))
            {
                if (_navPath.corners.Length > 1)
                {
                    MoveToPoint(_navPath.corners[1]); // Ir al siguiente punto del camino
                }
            }

            // Verificar distancia al target
            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (!(distanceToTarget <= stoppingDistance)) return;
            _controller.SimpleMove(Vector3.zero);
            return;
        }

        private void MoveToPoint(Vector3 targetPoint)
        {
            // Calcular dirección
            var direction = (targetPoint - transform.position).normalized;

            // Rotar hacia el punto
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime);
            }

            // Mover usando SimpleMove
            var moveDirection = transform.forward * speed;
            _controller.SimpleMove(moveDirection);
        }

        // Para debug: dibujar el camino en la escena
        private void OnDrawGizmos()
        {
            if (_navPath == null || _navPath.corners.Length <= 0) return;
            
            Gizmos.color = Color.red;
            for (var i = 0; i < _navPath.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(_navPath.corners[i], _navPath.corners[i + 1]);
            }
        }
    }
}