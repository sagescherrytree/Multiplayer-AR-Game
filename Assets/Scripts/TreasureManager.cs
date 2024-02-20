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
            Debug.Log("Start");
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("We are connected");
            }
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Master client");
                StartCoroutine(Spawn());
                StartCoroutine(SpawnPositiveBuff());
                StartCoroutine(SpawnNegativeEffect());
            }
            else
            {
                Debug.Log("Not Master Client");
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

        /*
        private void Update()
        {
            if (Pointer.current == null || !this.CanPlace)
                return;

            var touchPosition = Pointer.current.position.ReadValue();

            // Ensure we are not over any UI element.
            var uiButtons = FindObjectOfType<UIButtons>();
            if (uiButtons != null && (uiButtons.IsPointOverUI(touchPosition)))
                return;

            // Raycast against layer "GroundPlane" using normal Raycasting for our artifical ground plane.
            // For AR Foundation planes (if enabled), we use AR Raycasting.
            var ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("GroundPlane")))
            {
                this.CreateOrUpdateObject(hit.point, hit.transform.rotation);
            }
            else if (this.m_RaycastManager.Raycast(touchPosition, TreasureManager.s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = TreasureManager.s_Hits[0].pose;
                this.CreateOrUpdateObject(hitPose.position, hitPose.rotation);
            }
        }*/

        // Creates object once raycast successful.
        // Call this for each raycast for each object to spawn.
        /*private void CreateOrUpdateObject(Vector3 position, Quaternion rotation)
        {
            if (this.SpawnedObject == null)
            {
                this.SpawnedObject = PhotonNetwork.Instantiate(this.plantPrefab.name, position, rotation);
            }
            else
            {
                this.SpawnedObject.transform.position = position;
            }
        }*/
    }
}
