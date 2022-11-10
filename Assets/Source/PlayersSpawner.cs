using UnityEngine;

namespace Dasher
{
    internal class PlayersSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform[] spawnPoints;

        [SerializeField]
        private Player playerGameObject;

        private readonly System.Random random = new System.Random();

        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            for (int i = 0; i < 5; i++)
            {
                var player = Instantiate(playerGameObject, spawnPoints.GetRandomItem(random).position, Quaternion.identity);
                if (i != 4)
                    player.SetActiveInput(false);
            }
        }
    }
}