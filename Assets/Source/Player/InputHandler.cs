using UnityEngine;

namespace Dasher
{
    internal class InputHandler : MonoBehaviour
    {
        public static float MouseAxisX => PauseMenuUI.IsPauseNow ? 0 : Input.GetAxis("Mouse X");
        public static float MouseAxisY => PauseMenuUI.IsPauseNow ? 0 : Input.GetAxis("Mouse Y");
        public static bool DashPressed => !PauseMenuUI.IsPauseNow && Input.GetMouseButtonDown(0);

        public float HorizontalSmoothAxis { get; private set; }
        public float VerticalSmoothAxis { get; private set; }

        private float velocityHorizontal, velocityVertical;
        private const float SmoothTime = .1f;

        private void Update()
        {
            var inputHorizontal = PauseMenuUI.IsPauseNow ? 0 : Input.GetAxis("Horizontal");
            var inputVertical = PauseMenuUI.IsPauseNow ? 0 : Input.GetAxis("Vertical");
            HorizontalSmoothAxis = Mathf.SmoothDamp(HorizontalSmoothAxis, inputHorizontal, ref velocityHorizontal, SmoothTime);
            VerticalSmoothAxis = Mathf.SmoothDamp(VerticalSmoothAxis, inputVertical, ref velocityVertical, SmoothTime);
        }
    }
}