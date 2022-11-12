using UnityEngine;

namespace Dasher
{
    public class CameraFollow : MonoBehaviour
    {
        private Transform target;

        private Vector3 offsetPosition;

        private void Awake()
        {
            offsetPosition = transform.position;
        }

        private void Update()
        {
            if (!target)
                return;

            transform.position = target.TransformPoint(offsetPosition);
            transform.LookAt(target);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}