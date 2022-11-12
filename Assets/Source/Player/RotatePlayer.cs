using Mirror;
using UnityEngine;

namespace Dasher
{
    [RequireComponent(typeof(InputHandler))]
    internal class RotatePlayer : NetworkBehaviour
    {
        [SerializeField]
        private float rotationSpeed = 150;

        private InputHandler inputHandler;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;

            Rotate();
        }

        private void Rotate()
        {
            float mouseRotation = inputHandler.MouseAxisX * Time.deltaTime * rotationSpeed;
            transform.Rotate(Vector3.up, mouseRotation);
        }
    }
}