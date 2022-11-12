using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Dasher
{
    internal class LeaveButtonUI : MonoBehaviour
    {
        [SerializeField]
        private NetworkManager networkManager;

        [SerializeField]
        private Button leaveButton;

        private void Start()
        {
            leaveButton.AddSingleAction(ButtonPressed);
        }

        private void Update()
        {
            leaveButton.gameObject.SetActive(NetworkServer.active || NetworkClient.active);
        }

        private void ButtonPressed()
        {
            if (NetworkServer.active)
                networkManager.StopHost();
            else if (NetworkClient.active)
                networkManager.StopClient();
        }
    }
}