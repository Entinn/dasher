using Mirror;
using UnityEngine;

namespace Dasher
{
    [RequireComponent(typeof(InputHandler))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SkinColorChanger))]
    internal class Player : NetworkBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float speed = 1;

        [SerializeField]
        private float dashTime = 2;

        [SerializeField]
        private float dashSpeed = 5;

        [HideInInspector]
        [SyncVar(hook = nameof(UpdateScoreTable1))]
        public string Nickname;

        [HideInInspector]
        [SyncVar(hook = nameof(UpdateScoreTable2))]
        public int Score;

        private InputHandler inputHandler;
        private CharacterController characterController;
        private Animator animator;
        private SkinColorChanger skinColorChanger;

        private Timer dashTimer;
        private Timer damageTakenTimer;

        private Vector3 dashDirection;

        private bool active = true;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            skinColorChanger = GetComponent<SkinColorChanger>();
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

            GameManager.Instance.AddPlayer(this);
        }

        private void ChangeCharacterColor(bool active)
        {
            if (active)
                skinColorChanger.SetColor(Color.red);
            else
                skinColorChanger.ReturnToStartColor();
        }

        private void Update()
        {
            dashTimer.Service(Time.deltaTime);
            damageTakenTimer.Service(Time.deltaTime);

            if (!isLocalPlayer || !active)
                return;

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
            var attacker = GameManager.Instance.GetPlayerByConnectionID(attackerNetId);
            var target = GameManager.Instance.GetPlayerByConnectionID(targetNetId);
            //validation should be here
            if (target.damageTakenTimer.IsActive)
                return;

            GameManager.Instance.CmdAddScore(attackerNetId);

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
            var target = GameManager.Instance.GetPlayerByConnectionID(targetNetId);
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
            active = false;
            characterController.enabled = false;
            animator.SetFloat("VelocityX", 0);
            animator.SetFloat("VelocityZ", 0);
            animator.SetBool("Win", win);
            animator.SetBool("Lose", !win);
        }

        [ClientRpc]
        public void RpcReleaseCharacter()
        {
            active = true;
            characterController.enabled = true;
            animator.SetBool("Win", false);
            animator.SetBool("Lose", false);
        }

        private void OnDestroy()
        {
            GameManager.Instance.RemovePlayer(this);
        }

        private void UpdateScoreTable1(string oldValue, string newValue)
        {
            GameManager.Instance.UpdateScore();
        }

        private void UpdateScoreTable2(int oldValue, int newValue)
        {
            GameManager.Instance.UpdateScore();
        }
    }
}