using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFirstARGame
{
    public class ResetUI : MonoBehaviour
    {
        public void ResetGame()
        {
            var networkCommunication = FindObjectOfType<NetworkCommunication>();
            networkCommunication.AddPlayer();
            gameObject.SetActive(false);
        }
    }
}
