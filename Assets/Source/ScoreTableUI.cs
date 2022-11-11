using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dasher
{
    internal class ScoreTableUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;

        private readonly List<TextMeshProUGUI> scoreItems = new List<TextMeshProUGUI>();

        private void Start()
        {
            scoreText.gameObject.SetActive(false);
        }

        public void Update(Dictionary<string, int> score)
        {
            scoreItems.ForEach(x => Destroy(x.gameObject));
            scoreItems.Clear();

            foreach (var player in score)
            {
                var scoreItem = scoreText.Clone();
                scoreItem.text = $"{player.Key}: {player.Value}";
                scoreItems.Add(scoreItem);
            }
        }
    }
}