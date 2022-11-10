using UnityEngine;

namespace Dasher
{
    [RequireComponent(typeof(InputHandler), typeof(CharacterController), typeof(Animator))]
    internal class Player : MonoBehaviour
    {
        [SerializeField]
        private float speed = 1;

        [SerializeField]
        private float dashTime = 2;

        [SerializeField]
        private float dashSpeed = 5;

        [SerializeField]
        private float rotationSpeed = 5;

        private InputHandler inputHandler;
        private CharacterController characterController;
        private Animator animator;

        private float dashCurrentTime;
        private bool dashing;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Camera.main.transform.SetParent(transform);
        }

        private void Update()
        {
            float mouseRotation = inputHandler.MouseAxisX * Time.deltaTime * rotationSpeed;
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

            if (inputHandler.DashPressed)
            {
                dashing = true;
                Move(new Vector3(0, 0, dashSpeed));
                animator.SetBool("Dash", dashing);
                return;
            }

            var x = inputHandler.HorizontalSmoothAxis;
            var z = inputHandler.VerticalSmoothAxis;

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