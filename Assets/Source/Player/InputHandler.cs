using UnityEngine;

namespace Dasher
{
    internal class InputHandler : MonoBehaviour
    {
        public float MouseAxisX => Input.GetAxis("Mouse X");
        public bool DashPressed => Input.GetMouseButtonDown(0);

        public float HorizontalSmoothAxis => horizontalSmoothAxis;
        public float VerticalSmoothAxis => verticalSmoothAxis;

        private float horizontalSmoothAxis;
        private float verticalSmoothAxis;

        private float velocityHorizontal, velocityVertical;
        private const float SmoothTime = .1f;

        private void Update()
        {
            var inputHorizontal = Input.GetAxis("Horizontal");
            var inputVertical = Input.GetAxis("Vertical");
            horizontalSmoothAxis = Mathf.SmoothDamp(horizontalSmoothAxis, inputHorizontal, ref velocityHorizontal, SmoothTime);
            verticalSmoothAxis = Mathf.SmoothDamp(verticalSmoothAxis, inputVertical, ref velocityVertical, SmoothTime);
        }
    }
}