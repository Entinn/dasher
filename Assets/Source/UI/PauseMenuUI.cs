using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Dasher
{
    [RequireComponent(typeof(Canvas))]
    internal class PauseMenuUI : MonoBehaviour
    {
        [SerializeField]
        private NetworkManager networkManager;

        [SerializeField]
        private Button leaveButton;

        private Canvas canvas;

        private bool isEnabled;

        public static bool IsPauseNow;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            leaveButton.AddSingleAction(ButtonPressed);
        }

        private void Update()
        {
            if (!DasherNetManager.InGameNow && isEnabled)
            {
                isEnabled = false;
            }
            else if (DasherNetManager.InGameNow && Input.GetKeyDown(KeyCode.Escape))
            {
                isEnabled = !isEnabled;
            }

            bool setActive = DasherNetManager.InGameNow && isEnabled;
            canvas.enabled = setActive;
            IsPauseNow = setActive;
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