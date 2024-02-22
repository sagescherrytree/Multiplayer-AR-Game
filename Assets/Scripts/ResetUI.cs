using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFirstARGame
{
    public class ResetUI : MonoBehaviour
    {
        [SerializeField] private GameObject winLabel;
        [SerializeField] private GameObject loseLabel;
        public void SetText(bool won)
        {
            winLabel.SetActive(won);
            loseLabel.SetActive(!won);
        }
        
        public void ResetGame()
        {
            var networkCommunication = FindObjectOfType<NetworkCommunication>();
            networkCommunication.AddPlayer();
            gameObject.SetActive(false);
        }
    }
}
