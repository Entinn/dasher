using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dasher
{
    internal class ScoreTableUI : MonoBehaviour
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        private readonly List<TextMeshProUGUI> scoreItems = new List<TextMeshProUGUI>();

        private void Start()
        {
            scoreText.gameObject.SetActive(false);
            background.enabled = false;
        }

        public void UpdateScore(IDictionary<string, int> score)
        {
            scoreItems.ForEach(x => Destroy(x.gameObject));
            scoreItems.Clear();

            foreach (var player in score)
            {
                var scoreItem = scoreText.Clone();
                scoreItem.text = $"{player.Key}: {player.Value}";
                scoreItems.Add(scoreItem);
            }

            background.enabled = score.Count != 0;
        }
    }
}