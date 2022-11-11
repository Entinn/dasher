using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

namespace Dasher
{
    internal class Main : NetworkBehaviour
    {
        private readonly List<Player> players = new List<Player>();
        public IReadOnlyList<Player> Players => players;

        [SerializeField]
        private NetworkManager networkManager;

        [SerializeField]
        private WinnerTextUI winnerText;

        [SerializeField]
        private ScoreTableUI scoreTableUI;

        [SerializeField]
        private float winScreenTime = 5;

        [SerializeField]
        private int winScore = 3;

        public static Main Instance { get; private set; }

        protected void Awake()
        {
            Instance = this;
        }

        public void CmdAddScore(uint attackerNetId)
        {
            var attacker = GetPlayerByConnectionID(attackerNetId);
            attacker.Score++;
            if (attacker.Score >= winScore)
            {
                winnerText.SetWinner(attacker.Nickname);
                StartCoroutine(WaitForRestart(winScreenTime));
            }
        }

        IEnumerator WaitForRestart(float seconds)
        {
            foreach (var player in Players)
            {
                player.SetActive(false);
            }

            yield return new WaitForSeconds(seconds);
            winnerText.Clear();

            foreach (var player in Players)
            {
                player.Score = 0;
                var spawnPosition = networkManager.GetStartPosition().position;
                player.SetNewPosition(spawnPosition);
                player.SetActive(true);
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
            scoreTableUI?.UpdateScore(Players);
        }

        public Player GetPlayerByConnectionID(uint netId)
        {
            return Players.First(x => x.netId == netId);
        }
    }
}