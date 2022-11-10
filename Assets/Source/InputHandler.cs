using UnityEngine;

namespace Dasher
{
    internal class InputHandler : MonoBehaviour
    {
        public float MouseAxisX => active ? Input.GetAxis("Mouse X") : 0;
        public bool DashPressed => active && Input.GetKeyDown(KeyCode.LeftShift);

        public float HorizontalSmoothAxis => active ? horizontalSmoothAxis : 0;
        public float VerticalSmoothAxis => active ? verticalSmoothAxis : 0;

        private float horizontalSmoothAxis;
        private float verticalSmoothAxis;

        private float velocityHorizontal, velocityVertical;
        private const float SmoothTime = .1f;

        private bool active = true;

        private void Update()
        {
            var inputHorizontal = Input.GetAxis("Horizontal");
            var inputVertical = Input.GetAxis("Vertical");
            horizontalSmoothAxis = Mathf.SmoothDamp(horizontalSmoothAxis, inputHorizontal, ref velocityHorizontal, SmoothTime);
            verticalSmoothAxis = Mathf.SmoothDamp(verticalSmoothAxis, inputVertical, ref velocityVertical, SmoothTime);
        }

        public void SetActive(bool active)
        {
            this.active = active;
        }
    }
}