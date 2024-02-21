using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MyFirstARGame
{
    public class DisconnectObject : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.Disconnect();
        }
    }
}
