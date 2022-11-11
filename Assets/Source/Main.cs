using Mirror;
using UnityEngine;

namespace Dasher
{
    internal class Main : NetworkBehaviour
    {
        [SerializeField]
        private ScoreTableUI scoreTableUI;

        public static Main Instance { get; private set; }

        private readonly SyncDictionary<string, int> playersScore = new SyncDictionary<string, int>();

        protected void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            playersScore.Callback += (a,b,c) => OnScoreUpdate();
    
        }

        public void AddScore(string player)
        {
            playersScore[player] += 1;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            playersScore.Callback += (a,b,c) => OnScoreUpdate();
            CmdAddNickname(LoginUI.Nickname);
        }

        [Command(requiresAuthority = false)]
        private void CmdAddNickname(string nickname)
        {
            playersScore.Add(nickname, 0);
        }

        private void OnScoreUpdate()
        {
            scoreTableUI.UpdateScore(playersScore);
        }
    }
}