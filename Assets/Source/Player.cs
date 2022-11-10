using UnityEngine;

namespace Dasher
{
    internal class Player : MonoBehaviour
    {
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float speed = 1;

        [SerializeField]
        private float smoothTime = 1;

        [SerializeField]
        private float dashTime = 2;

        [SerializeField]
        private float dashSpeed = 5;

        private float x, z;
        private float velocityX, velocityZ;
        private float dashCurrentTime = 0;
        private bool dashing;

        private void Update()
        {
            if (dashing)
            {
                characterController.Move(new Vector3(0, 0, dashSpeed) * (Time.deltaTime * speed));
                dashCurrentTime += Time.deltaTime;
                if (dashCurrentTime >= dashTime)
                {
                    dashCurrentTime = 0;
                    dashing = false;
                    animator.SetBool("Dash", dashing);
                }

                return;
            }

            var localX = Input.GetAxis("Horizontal");
            var localZ = Input.GetAxis("Vertical");
            x = Mathf.SmoothDamp(x, localX, ref velocityX, smoothTime);
            z = Mathf.SmoothDamp(z, localZ, ref velocityZ, smoothTime);

            animator.SetFloat("VelocityX", x);
            animator.SetFloat("VelocityZ", z);

            characterController.Move(new Vector3(x, 0, z) * (Time.deltaTime * speed));

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dashing = true;
                characterController.Move(new Vector3(0, 0, dashSpeed) * (Time.deltaTime * speed));
                animator.SetBool("Dash", dashing);
            }
        }
    }
}