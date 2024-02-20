namespace MyFirstARGame
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;
    using Photon.Pun;

    /// <summary>
    /// Spawns a plant obj at a certain random point in the screen. 
    /// </summary>

    //[RequireComponent(typeof(Camera))]
    public class TreasureManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        public GameObject plantPrefab;
        [SerializeField]
        public GameObject positiveBuffPrefab;
        [SerializeField]
        public GameObject negativeEffectPrefab;

        [SerializeField]
        int numPlants;
        [SerializeField]
        int numPositiveBuffs;
        [SerializeField]
        int numNegativeEffects;

        [SerializeField]
        float spawnRate;

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(Spawn());
                StartCoroutine(SpawnPositiveBuff());
                StartCoroutine(SpawnNegativeEffect());
            }
        }


        public System.Collections.IEnumerator Spawn()
        {
            while (true)
            {
                for (int i = 0; i < numPlants; i++)
                {
                    Vector3 spawnPoint = new(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f));
                    PhotonNetwork.Instantiate("Plant", spawnPoint, Quaternion.identity, data: new object[] { });
                }

                yield return new WaitForSeconds(spawnRate);
            }

        }

        public System.Collections.IEnumerator SpawnPositiveBuff()
        {
            while (true)
            {
                for (int i = 0; i < numPositiveBuffs; i++)
                {
                    Vector3 spawnPoint = new(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f));
                    PhotonNetwork.Instantiate("PositivePowerUp", spawnPoint, Quaternion.identity, data: new object[] { });
                }
                yield return new WaitForSeconds(spawnRate);
            }
        }
        public System.Collections.IEnumerator SpawnNegativeEffect()
        {
            while (true)
            {
                for (int i = 0; i < numNegativeEffects; i++)
                {
                    Vector3 spawnPoint = new(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f));
                    PhotonNetwork.Instantiate("NegativePenalty", spawnPoint, Quaternion.identity, data: new object[] { });
                }
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }
}
