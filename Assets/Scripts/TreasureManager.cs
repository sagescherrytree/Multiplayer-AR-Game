using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MyFirstARGame
{
    /// <summary>
    ///     Spawns a plant obj at a certain random point in the screen.
    /// </summary>

    //[RequireComponent(typeof(Camera))]
    public class TreasureManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] public GameObject plantPrefab;

        [SerializeField] public GameObject positiveBuffPrefab;

        [SerializeField] public GameObject negativeEffectPrefab;

        [SerializeField] private int numPlants;

        [SerializeField] private int numPositiveBuffs;

        [SerializeField] private int numNegativeEffects;

        [SerializeField] private float spawnRate;

        private HashSet<int> _spawnedItems = new();

        [SerializeField] private bool _isRunning = false;

        public void StartGame()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                StartCoroutine(SpawnCoroutine());
            }
            _isRunning = true;
        }

        private void Spawn(string resource)
        {
            Vector3 spawnPoint = new(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f));
            var photonViewId = PhotonNetwork
                .Instantiate(resource, spawnPoint, Quaternion.identity, data: new object[] { })
                .GetComponent<PhotonView>()
                .ViewID;
            _spawnedItems.Add(photonViewId);
        }

        private IEnumerator SpawnCoroutine()
        {
            while (_isRunning)
            {
                for (var i = 0; i < numPlants; i++)
                {
                    Spawn("Plant");
                }

                for (var i = 0; i < numPositiveBuffs; i++)
                {
                    Spawn("PositivePowerUp");
                }

                for (var i = 0; i < numNegativeEffects; i++)
                {
                    Spawn("NegativePenalty");
                }

                yield return new WaitForSeconds(spawnRate);
            }
        }

        public void DestroyItem(int photonViewId)
        {
            _spawnedItems.Remove(photonViewId);
            var itemPhotonView = PhotonView.Find(photonViewId);
            if (itemPhotonView != null)
            {
                PhotonNetwork.Destroy(itemPhotonView);
            }
        }

        public void Stop()
        {
            _isRunning = false;
            foreach (int itemId in _spawnedItems)
            {
                var itemPhotonView = PhotonView.Find(itemId);
                if (itemPhotonView != null)
                {
                    PhotonNetwork.Destroy(itemPhotonView);
                }
            }
            _spawnedItems.Clear();
        }
    }
}