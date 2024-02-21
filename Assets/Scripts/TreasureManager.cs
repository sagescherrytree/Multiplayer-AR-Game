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
        public GameObject goldenSnitch;

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
            StartCoroutine(Spawn());
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
                for (int i = 0; i < numPositiveBuffs; i++)
                {
                    Vector3 spawnPoint = new(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f));
                    PhotonNetwork.Instantiate("PositivePowerUp", spawnPoint, Quaternion.identity, data: new object[] { });
                }
                for (int i = 0; i < numNegativeEffects; i++)
                {
                    Vector3 spawnPoint = new(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f));
                    PhotonNetwork.Instantiate("NegativePenalty", spawnPoint, Quaternion.identity, data: new object[] { });
                }
                
                // golden snitch
                if (Random.Range(0f, 1f) < 0.2) {
                    Vector3 spawnPoint = new(Random.Range(-5f, 5f), 1, Random.Range(-5f,5f));
                    PhotonNetwork.Instantiate("Snitch", spawnPoint, Quaternion.identity, data: new object[] { });
                }
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }
}
