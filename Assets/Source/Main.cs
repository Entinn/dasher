using System.Collections.Generic;
using UnityEngine;

namespace Dasher
{
    internal class Main : MonoBehaviour
    {
        [SerializeField]
        private PlayersSpawner playersSpawner;

        [SerializeField]
        private ScoreTableUI scoreTableUI;

        private readonly Dictionary<string, int> playersScore = new Dictionary<string, int>();

        public void AddScore(string player)
        {
            playersScore.CreateOrAdd(player, 1);
            scoreTableUI.Update(playersScore);
        }
    }
}