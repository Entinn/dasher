using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dasher
{
    internal class LoginUI : MonoBehaviour
    {
        public static string Nickname;

        [SerializeField]
        private DasherNetManager netManager;

        [SerializeField]
        private TMP_InputField server, nickname;

        [SerializeField]
        private Button host, connect;

        private void Start()
        {
            server.onValueChanged.AddListener(OnServerChanged);
            nickname.onValueChanged.AddListener(OnNicknameChanged);

            OnServerChanged("localhost");
            OnNicknameChanged("player" + Random.Range(0, 10000));

            host.AddSingleAction(OnHostPressed);
            connect.AddSingleAction(OnConnectPressed);
        }

        private void OnServerChanged(string server)
        {
            if (string.IsNullOrWhiteSpace(server))
                return;

            netManager.SetHostName(server);
        }

        private void OnNicknameChanged(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            Nickname = name;
        }

        private void OnHostPressed()
        {
            netManager.StartHost();
            gameObject.SetActive(false);
        }

        private void OnConnectPressed()
        {
            netManager.StartClient();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            server.onValueChanged.RemoveAllListeners();
            nickname.onValueChanged.RemoveAllListeners();
        }
    }
}