using UnityEngine;

namespace Dasher
{
    internal class CursorManager : MonoBehaviour
    {
        private void Update()
        {
            if (!DasherNetManager.InGameNow)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (DasherNetManager.InGameNow && PauseMenuUI.IsPauseNow)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else if (DasherNetManager.InGameNow && !PauseMenuUI.IsPauseNow)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}