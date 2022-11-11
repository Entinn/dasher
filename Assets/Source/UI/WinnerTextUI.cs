using Mirror;
using TMPro;
using UnityEngine;

namespace Dasher
{
    internal class WinnerTextUI : NetworkBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI winnerText;

        private void Start()
        {
            winnerText.text = string.Empty;
        }

        [ClientRpc]
        public void SetWinner(string playerName)
        {
            winnerText.text = "Winner: " + playerName;
        }

        [ClientRpc]
        public void Clear()
        {
            winnerText.text = string.Empty;
        }
    }
}