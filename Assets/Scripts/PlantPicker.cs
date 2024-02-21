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

                    // end game and compare w/ other players' points to check if won/lost
                    // var currPName = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
                    // var currScore = networkCommunication.scoreboard.GetScore(currPName);
                    // bool lost = false;

                    // foreach (var p in PhotonNetwork.PlayerList) {
                    //     var pName = $"Player {p.ActorNumber}";
                    //     if (networkCommunication.scoreboard.GetScore(pName) > currScore) {
                    //         lost = true;
                    //     }
                    // }

                    // if (lost) {
                    //     SceneManager.LoadScene("Game_Over");
                    // } else {
                    //     SceneManager.LoadScene("Victory");
                    // }
                    break;
                }
                default:
                {
                    return;
                }
            }
            
            networkCommunication.DestroyItem(selectedObject.GetComponent<PhotonView>().ViewID);
            networkCommunication.ChangeScore(points);
            
            if (networkCommunication.GetCurrentScore() >= maxScore)
            {
                networkCommunication.GameOver();
                SceneManager.LoadScene("Victory");
            }
        }
    }
}