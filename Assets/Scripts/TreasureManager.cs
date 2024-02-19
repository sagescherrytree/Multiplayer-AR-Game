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

    [RequireComponent(typeof(Camera))]
    public class TreasureManager : PressInputBase
    {
        [SerializeField]
        public GameObject plantPrefab;

        // Number of plants/
        [SerializeField]
        float numPlants;

        private ARRaycastManager m_RaycastManager;

        private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject SpawnedObject { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to place an object.
        /// </summary>
        public bool CanPlace { get; set; }

        protected override void Awake()
        {
            base.Awake();
            this.m_RaycastManager = this.GetComponent<ARRaycastManager>();
        }

        private void Spawn()
        {
            // Spawns plant prefabs at certain time intervals.
            // Num plants.
            // Set of random directions.
            // For each direction cast a ray 
            // Test if raycast hits groundplane
            for (int i = 0; i < numPlants; i++) {
                Vector3 randomScreenPt = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0f);
                var ray = Camera.main.ScreenPointToRay(randomScreenPt);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("GroundPlane")))
                {
                    this.CreateOrUpdateObject(hit.point, hit.transform.rotation);
                }
                else if (this.m_RaycastManager.Raycast(ray, TreasureManager.s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    var hitPose = TreasureManager.s_Hits[0].pose;
                    this.CreateOrUpdateObject(hitPose.position, hitPose.rotation);
                }
            }
        }

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
        }

        // Creates object once raycast successful.
        // Call this for each raycast for each object to spawn.
        private void CreateOrUpdateObject(Vector3 position, Quaternion rotation)
        {
            if (this.SpawnedObject == null)
            {
                this.SpawnedObject = PhotonNetwork.Instantiate(this.plantPrefab.name, position, rotation);
            }
            else
            {
                this.SpawnedObject.transform.position = position;
            }
        }
    }
}
