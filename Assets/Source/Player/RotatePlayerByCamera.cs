using Mirror;
using UnityEngine;

namespace Dasher
{
    internal class RotatePlayerByCamera : NetworkBehaviour
    {
        private CameraMovement characterCamera;

        private void Awake()
        {
            characterCamera = Camera.main.GetComponent<CameraMovement>();
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;

            Rotate();
        }

        private void Rotate()
        {
            var eulerAngles = transform.eulerAngles;
            Vector3 newRotation = new Vector3(eulerAngles.x, characterCamera.transform.eulerAngles.y, eulerAngles.z);
            gameObject.transform.eulerAngles = newRotation;
        }
    }
}