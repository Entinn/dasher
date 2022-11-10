using UnityEngine;

namespace Dasher
{
    internal class InputHandler : MonoBehaviour
    {
        public float MouseAxisX => Input.GetAxis("Mouse X");
        public float HorizontalSmoothAxis { get; private set; }
        public float VerticalSmoothAxis { get; private set; }
        public bool DashPressed => Input.GetMouseButtonDown(0);

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