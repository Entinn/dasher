using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

namespace Dasher
{
    internal class GameManager : NetworkBehaviour
    {
        private readonly List<Player> players = new List<Player>();

        [Header("Links")]
        [SerializeField]
        private NetworkManager networkManager;

        [SerializeField]
        private WinnerTextUI winnerText;

        [SerializeField]
        private ScoreTableUI scoreTableUI;

        [Header("Settings")]
        [SerializeField]
        private float winScreenTime = 5;

        [SerializeField]
        private int winScore = 3;

        private static GameManager instance;
        /// <summary>
        /// Dependency injection better than this
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    Debug.LogError("GameManager instance not initialized");
                return instance;
            }
        }

        protected void Awake()
        {
            if (instance != null)
                Debug.LogError("GameManager multiple instances in scene");

            instance = this;
        }

        public void CmdAddScore(uint attackerNetId)
        {
            var attacker = GetPlayerByConnectionID(attackerNetId);
            attacker.Score++;
            if (attacker.Score >= winScore)
            {
                winnerText.SetWinner(attacker.Nickname);
                StartCoroutine(WaitForRestart(winScreenTime, attacker));
            }
        }

        IEnumerator WaitForRestart(float seconds, Player winner)
        {
            foreach (var player in players)
            {
                player.RpcGameOver(player == winner);
            }

            yield return new WaitForSeconds(seconds);
            winnerText.Clear();

            foreach (var player in players)
            {
                player.Score = 0;
                var spawnPosition = networkManager.GetStartPosition().position;
#if UNITY_EDITOR
                Debug.Log($"Send {nameof(player.RpcSetNewPosition)} for player {player.Nickname}");
#endif
                player.RpcSetNewPosition(spawnPosition);
                player.RpcReleaseCharacter();
            }
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
            UpdateScore();
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
            UpdateScore();
        }

        public void UpdateScore()
        {
            scoreTableUI?.UpdateScore(players);
        }

        public Player GetPlayerByConnectionID(uint netId)
        {
            return players.First(x => x.netId == netId);
        }
    }
}