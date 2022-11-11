using Mirror;
using UnityEngine;

namespace Dasher
{
    [RequireComponent(typeof(InputHandler), typeof(CharacterController), typeof(Animator))]
    internal class Player : NetworkBehaviour
    {
        [SerializeField]
        private float speed = 1;

        [SerializeField]
        private float dashTime = 2;

        [SerializeField]
        private float dashSpeed = 5;

        [SerializeField]
        private float rotationSpeed = 5;

        [SerializeField]
        private Renderer changeColorMaterial;

        private InputHandler inputHandler;
        private CharacterController characterController;
        private Animator animator;

        private Timer dashTimer;
        private Timer damageTakenTimer;

        private Vector3 dashDirection;

        private Color baseRendererColor;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            Camera.main.transform.SetParent(transform, false);
            Cursor.visible = false;
        }

        private void Start()
        {
            dashTimer = new Timer(dashTime);
            dashTimer.OnActivityChanged += x => animator.SetBool("Dash", x);

            damageTakenTimer = new Timer(2);
            damageTakenTimer.OnActivityChanged += ChangeCharacterColor;

            baseRendererColor = changeColorMaterial.material.color;
        }

        private void ChangeCharacterColor(bool active)
        {
            changeColorMaterial.material.color = active ? Color.red : baseRendererColor;
        }

        private void Update()
        {
            dashTimer.Service(Time.deltaTime);
            damageTakenTimer.Service(Time.deltaTime);

            if (!isLocalPlayer)
                return;

            Rotate();

            if (inputHandler.DashPressed && !dashTimer.IsActive)
            {
                dashTimer.Start();
                dashDirection = transform.TransformDirection(new Vector3(0, 0, dashSpeed));
                return;
            }

            if (dashTimer.IsActive)
            {
                Move(dashDirection, false);
                return;
            }

            var x = inputHandler.HorizontalSmoothAxis;
            var z = inputHandler.VerticalSmoothAxis;

            animator.SetFloat("VelocityX", x);
            animator.SetFloat("VelocityZ", z);

            Move(new Vector3(x, 0, z));
        }

        private void Rotate()
        {
            float mouseRotation = inputHandler.MouseAxisX * Time.deltaTime * rotationSpeed;
            transform.Rotate(Vector3.up, mouseRotation);
        }

        private void Move(Vector3 motion, bool transformDirection = true)
        {
            var motionAccelerated = motion * (Time.deltaTime * speed);
            if (transformDirection)
                motionAccelerated = transform.TransformDirection(motionAccelerated);

            characterController.Move(motionAccelerated);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!dashTimer.IsActive || !isLocalPlayer)
                return;

            var anotherPlayer = hit.gameObject.GetComponent<Player>();

            if (anotherPlayer)
            {
                if (anotherPlayer.damageTakenTimer.IsActive)
                    return;

                CmdTakeDamage(anotherPlayer, LoginUI.Nickname);
            }
        }

        [Command]
        private void CmdTakeDamage(Player player, string attackerNickname)
        {
            if (player.damageTakenTimer.IsActive)
                return;

            Main.Instance.AddScore(attackerNickname);
            player.damageTakenTimer.Start();
            player.TakeDamage();
        }

        [ClientRpc]
        private void TakeDamage()
        {
            damageTakenTimer.Start();
        }
    }
}