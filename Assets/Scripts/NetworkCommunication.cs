using System.Collections;
using System.Threading;
using UnityEngine.Serialization;

namespace MyFirstARGame
{
    using Photon.Pun;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// You can use this class to make RPC calls between the clients. It is already spawned on each client with networking capabilities.
    /// </summary>
    public class NetworkCommunication : MonoBehaviourPun
    {
        [SerializeField]
        public Scoreboard scoreboard;

        [SerializeField]
        private TreasureManager treasureManagerPrefab;

        private TreasureManager treasureManagerInstance = null;

        [SerializeField]
        public GameObject endGame;

        [SerializeField]
        private int numPlayers;
        
        
        // Start is called before the first frame update
        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                treasureManagerInstance = Instantiate(treasureManagerPrefab).GetComponent<TreasureManager>();
            }
            AddPlayer();
        }

        public void AddPlayer()
        {
            this.photonView.RPC("Network_AddPlayer", RpcTarget.All);
        }

        [PunRPC]
        public void Network_AddPlayer()
        {
            numPlayers++;
            if (numPlayers >= 2)
            {
                this.photonView.RPC("Network_StartGame", RpcTarget.MasterClient);
            }
        }

        [PunRPC]
        private void Network_StartGame()
        {
            treasureManagerInstance.StartGame();
        }

        public void ChangeScore(int delta)
        {
            var playerName = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = scoreboard.GetScore(playerName);
            this.photonView.RPC("Network_SetPlayerScore", RpcTarget.All, playerName, currentScore + delta);
        }

        public int GetCurrentScore()
        {
            // Gets score for current player.
            var playerName = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(playerName);
            return currentScore;
        }
        
        [PunRPC]
        public void Network_SetPlayerScore(string playerName, int newScore)
        {
            Debug.Log($"Player {playerName} scored!");
            this.scoreboard.SetScore(playerName, newScore);
        }

        public void DestroyItem(int viewId)
        {
            photonView.RPC("Network_DestroyItem", RpcTarget.MasterClient, viewId);
        }
        
        [PunRPC]
        public void Network_DestroyItem(int viewId)
        {
            treasureManagerInstance.DestroyItem(viewId);
        }
        
        public void EndGame(int winner)
        {
            photonView.RPC("Network_EndGame", RpcTarget.All, winner);
        }

        public void DebugMessage(string s)
        {
            photonView.RPC("Network_DebugMessage", RpcTarget.MasterClient, s);
        }

        [PunRPC]
        private void Network_DebugMessage(string s)
        {
            Debug.Log($"Debug Message: {s}");
        }


        public void Reset(int playerNum)
        {
            // Reset values in scoreboard.
            this.scoreboard.Clear();
            // Restart the treas
        }

        [PunRPC]
        private void Network_EndGame(int winner)
        {
            numPlayers = 0;
            scoreboard.Clear();
            if (treasureManagerInstance != null)
            {
                treasureManagerInstance.Stop();
            }
            endGame.SetActive(true);
            // WinLossUI winLossManager = GameObject.FindObjectOfType<WinLossUI>();
            // winLossManager.showText(winner == PhotonNetwork.LocalPlayer.ActorNumber);
        }


        
        [PunRPC]
        public void UpdateForNewPlayer(Photon.Realtime.Player player)
        {
            // Send current player scores to new player.
            var playerName = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(playerName);
            this.photonView.RPC("Network_SetPlayerScore", player, playerName, currentScore);
        }
    }

}