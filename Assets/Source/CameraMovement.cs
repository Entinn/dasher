using UnityEngine;

namespace Dasher
{
    public class CameraMovement : MonoBehaviour
    {
        private const float YMin = 10;
        private const float YMax = 60;

        [SerializeField]
        private float distance = 10;

        [SerializeField]
        private float sensitivity = 150;

        private Transform target;

        private float currentX;
        private float currentY;

        private void LateUpdate()
        {
            if (!target)
                return;

            currentX += InputHandler.MouseAxisX * sensitivity * Time.deltaTime;
            currentY -= InputHandler.MouseAxisY * sensitivity * Time.deltaTime;

            currentY = Mathf.Clamp(currentY, YMin, YMax);

            Vector3 direction = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

            var targetPosition = target.position;
            transform.position = targetPosition + rotation * direction;

            transform.LookAt(targetPosition);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}