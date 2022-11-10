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

        [SerializeField]
        private float rotationSpeed = 5;

        private float x, z;
        private float velocityX, velocityZ;
        private float dashCurrentTime = 0;
        private bool dashing;

        private void Start()
        {
            Camera.main.transform.SetParent(transform);
        }

        private void Update()
        {
            float mouseRotation = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
            transform.Rotate(Vector3.up, mouseRotation);

            if (dashing)
            {
                Move(new Vector3(0, 0, dashSpeed));
                dashCurrentTime += Time.deltaTime;
                if (dashCurrentTime >= dashTime)
                {
                    dashCurrentTime = 0;
                    dashing = false;
                    animator.SetBool("Dash", dashing);
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dashing = true;
                Move(new Vector3(0, 0, dashSpeed));
                animator.SetBool("Dash", dashing);
                return;
            }

            var localX = Input.GetAxis("Horizontal");
            var localZ = Input.GetAxis("Vertical");
            x = Mathf.SmoothDamp(x, localX, ref velocityX, smoothTime);
            z = Mathf.SmoothDamp(z, localZ, ref velocityZ, smoothTime);

            animator.SetFloat("VelocityX", x);
            animator.SetFloat("VelocityZ", z);

            Move(new Vector3(x, 0, z));
        }

        private void Move(Vector3 motion)
        {
            characterController.Move(transform.TransformDirection(motion * (Time.deltaTime * speed)));
        }
    }
}