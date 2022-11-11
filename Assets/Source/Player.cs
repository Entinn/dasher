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

        [SyncVar(hook = nameof(UpdateScoreTable1))]
        public string Nickname;

        [SyncVar(hook = nameof(UpdateScoreTable2))]
        public int Score;

        private InputHandler inputHandler;
        private CharacterController characterController;
        private Animator animator;

        private Timer dashTimer;
        private Timer damageTakenTimer;

        private Vector3 dashDirection;

        private Color baseRendererColor;

        private bool active = true;

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
            CmdSetNickname(LoginUI.Nickname);
        }

        //not the best way to do this, better make through authenticator overriding
        [Command]
        private void CmdSetNickname(string nickname)
        {
            Nickname = nickname;
        }

        private void Start()
        {
            dashTimer = new Timer(dashTime);
            dashTimer.OnActivityChanged += x => animator.SetBool("Dash", x);

            damageTakenTimer = new Timer(2);
            damageTakenTimer.OnActivityChanged += ChangeCharacterColor;

            baseRendererColor = changeColorMaterial.material.color;

            Main.Instance.AddPlayer(this);
        }

        private void ChangeCharacterColor(bool active)
        {
            changeColorMaterial.material.color = active ? Color.red : baseRendererColor;
        }

        private void Update()
        {
            dashTimer.Service(Time.deltaTime);
            damageTakenTimer.Service(Time.deltaTime);

            if (!isLocalPlayer || !active)
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

        [Client]
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!isLocalPlayer)
                return;

            var anotherPlayer = hit.gameObject.GetComponent<Player>();

            if (!dashTimer.IsActive || anotherPlayer == null || anotherPlayer.damageTakenTimer.IsActive)
                return;

#if UNITY_EDITOR
            Debug.Log($"Sending {nameof(CmdTakeDamage)}");
#endif

            //local simulation
            if (!isServer)
                anotherPlayer.damageTakenTimer.Start();

            CmdTakeDamage(netId, anotherPlayer.netId);
        }

        [Command]
        private void CmdTakeDamage(uint attackerNetId, uint targetNetId)
        {
#if UNITY_EDITOR
            Debug.Log($"Received {nameof(CmdTakeDamage)}");
#endif
            var attacker = Main.Instance.GetPlayerByConnectionID(attackerNetId);
            var target = Main.Instance.GetPlayerByConnectionID(targetNetId);
            //validation should be here
            if (target.damageTakenTimer.IsActive)
                return;

            Main.Instance.CmdAddScore(attackerNetId);

#if UNITY_EDITOR
            Debug.Log($"Sending {nameof(RpcTakeDamage)}");
#endif

            RpcTakeDamage(targetNetId);
        }

        [ClientRpc]
        private void RpcTakeDamage(uint targetNetId)
        {
#if UNITY_EDITOR
            Debug.Log($"Received {nameof(RpcTakeDamage)}");
#endif
            var target = Main.Instance.GetPlayerByConnectionID(targetNetId);
            target.damageTakenTimer.Start();
        }

        [ClientRpc]
        public void RpcSetNewPosition(Vector3 newPos)
        {
#if UNITY_EDITOR
            Debug.Log($"Received {nameof(RpcSetNewPosition)}: {newPos}");
#endif
            GetComponent<NetworkTransform>().Reset();
            transform.position = newPos;
        }

        [ClientRpc]
        public void RpcGameOver(bool win)
        {
            this.active = false;
            characterController.enabled = false;
            animator.SetFloat("VelocityX", 0);
            animator.SetFloat("VelocityZ", 0);
            animator.SetBool("Win", win);
            animator.SetBool("Lose", !win);
        }

        [ClientRpc]
        public void RpcReleaseCharacter()
        {
            this.active = true;
            characterController.enabled = true;
            animator.SetBool("Win", false);
            animator.SetBool("Lose", false);
        }

        private void OnDestroy()
        {
            Main.Instance.RemovePlayer(this);
        }

        private void UpdateScoreTable1(string oldValue, string newValue)
        {
            Main.Instance.UpdateScore();
        }

        private void UpdateScoreTable2(int oldValue, int newValue)
        {
            Main.Instance.UpdateScore();
        }
    }
}