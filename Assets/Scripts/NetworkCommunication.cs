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
        
        
        // Start is called before the first frame update
        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                treasureManagerInstance = Instantiate(treasureManagerPrefab).GetComponent<TreasureManager>();
            }
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
        
        // Pun system gibberish that I do not understand.
        [PunRPC]
        public void Network_SetPlayerScore(string playerName, int newScore)
        {
            Debug.Log($"Player {playerName} scored!");
            this.scoreboard.SetScore(playerName, newScore);
        }

        public void DestroyItem(int viewId)
        {
            photonView.RPC("Network_DestroyPhotonView", RpcTarget.MasterClient, viewId);
        }
        
        [PunRPC]
        public void Network_DestroyPhotonView(int viewId)
        {
            treasureManagerInstance.DestroyItem(viewId);
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

        public void GameOver()
        {
            photonView.RPC("Network_GameOver", RpcTarget.Others);
        }

        [PunRPC]
        private void Network_GameOver()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                treasureManagerInstance.Stop();
                return;
            }
            SceneManager.LoadScene("Game_Over");
        }
        
        public void UpdateForNewPlayer(Photon.Realtime.Player player)
        {
            // Send current player scores to new player.
            var playerName = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(playerName);
            this.photonView.RPC("Network_SetPlayerScore", player, playerName, currentScore);
        }
    }

}