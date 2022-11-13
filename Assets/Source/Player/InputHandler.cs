using UnityEngine;

namespace Dasher
{
    internal class InputHandler : MonoBehaviour
    {
        public static float MouseAxisX => Input.GetAxis("Mouse X");
        public static float MouseAxisY => Input.GetAxis("Mouse Y");
        public static bool DashPressed => Input.GetMouseButtonDown(0);

        public float HorizontalSmoothAxis { get; private set; }
        public float VerticalSmoothAxis { get; private set; }

        private float velocityHorizontal, velocityVertical;
        private const float SmoothTime = .1f;

        private void Update()
        {
            var inputHorizontal = Input.GetAxis("Horizontal");
            var inputVertical = Input.GetAxis("Vertical");
            HorizontalSmoothAxis = Mathf.SmoothDamp(HorizontalSmoothAxis, inputHorizontal, ref velocityHorizontal, SmoothTime);
            VerticalSmoothAxis = Mathf.SmoothDamp(VerticalSmoothAxis, inputVertical, ref velocityVertical, SmoothTime);
        }
    }
}