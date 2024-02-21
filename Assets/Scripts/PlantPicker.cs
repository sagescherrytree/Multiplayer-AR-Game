namespace MyFirstARGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;
    using Photon.Pun;
    using UnityEngine.SceneManagement;
    using TMPro;

    /// <summary>
    /// Spawns a plant obj at a certain random point in the screen. 
    /// </summary>

    public class PlantPicker : PressInputBase
    {

        [SerializeField]
        public int maxScore = 10;

        // For particle effects!
        [SerializeField]
        public GameObject particleEffectPrefab;

        private ARRaycastManager m_RaycastManager;
        private bool pressed;

        RaycastHit hitData;
        GameObject selectedObject;

        protected override void Awake()
        {
            Debug.Log("Plant picker awake.");
            base.Awake();
            this.m_RaycastManager = this.GetComponent<ARRaycastManager>();
        }

        protected override void OnPressBegan(Vector3 position)
        {
            // Ensure we are not over any UI element.
            var uiButtons = FindObjectOfType<UIButtons>();
            if (uiButtons != null && (uiButtons.IsPointOverUI(position)))
            {
                return;
            }
            
            var ray = this.GetComponent<Camera>().ScreenPointToRay(position);
            
            if (!Physics.Raycast(ray, out hitData, 1000)) return;
            
            selectedObject = hitData.transform.gameObject;

            var networkCommunication = FindObjectOfType<NetworkCommunication>();
            
            networkCommunication.DebugMessage($"Hit a {selectedObject.tag}");

            int points;
            switch (selectedObject.tag)
            {
                case "plant":
                {
                    points = 1;
                    break;
                }
                case "positive":
                {
                    points = 5;
                    break;
                }
                case "negative":
                {
                    points = -5;
                    break;
                }
                case "snitch":
                {
                    points = 150;
                    break;
                }
                default:
                {
                    return;
                }
            }
            // Spawn partcles.
            //PhotonNetwork.Instantiate(particleEffectPrefab.name, hitData.point, Quaternion.identity);

            networkCommunication.DestroyItem(selectedObject.GetComponent<PhotonView>().ViewID);
            networkCommunication.ChangeScore(points);
            
            if (networkCommunication.GetCurrentScore() >= maxScore)
            {
                networkCommunication.EndGame(PhotonNetwork.LocalPlayer.ActorNumber);
                SceneManager.LoadScene("Victory");
            }
        }
    }
}