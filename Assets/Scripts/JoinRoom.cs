using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace MyFirstARGame
{
    public class JoinRoom : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Button button = GetComponent<Button>();

            // Add a listener for the button click event
            button.onClick.AddListener(() => PhotonNetwork.JoinRandomOrCreateRoom());
        }
    }
}
