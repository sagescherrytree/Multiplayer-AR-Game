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
        //MonobehaviorPunCallbacks

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
            var networkCommunication = FindObjectOfType<NetworkCommunication>();
            var ray = this.GetComponent<Camera>().ScreenPointToRay(position);
            if (Physics.Raycast(ray, out hitData, 1000))
            {
                Debug.Log("Cast ray.");
                selectedObject = hitData.transform.gameObject;

                if (selectedObject.tag == "plant")
                {
                    PhotonNetwork.Destroy(selectedObject);
                    networkCommunication.SetScoreText("Destroyed plant.");
                }
            }
            //networkCommunication.SetScoreText($"o: {ray.origin} d: {ray.direction}, hit: {hitData.transform.gameObject.tag}");
        }
    }
}