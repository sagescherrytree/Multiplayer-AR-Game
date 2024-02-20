namespace MyFirstARGame
{
    using Photon.Pun;
    using UnityEngine;

    /// <summary>
    /// You can use this class to make RPC calls between the clients. It is already spawned on each client with networking capabilities.
    /// </summary>
    public class NetworkCommunication : MonoBehaviourPun
    {
        [SerializeField]
        private Scoreboard scoreboard;
        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.Instantiate("TreasureManager", Vector3.zero, Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Increment score function.
        public void IncrementScore()
        {
            // Send to everyone, including self.
            var playerName = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(playerName);
            this.photonView.RPC("Network_SetPlayerScore", RpcTarget.All, playerName, currentScore + 1);
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

        public void DestroyPhotonView(int viewId)
        {
            photonView.RPC("Network_DestroyPhotonView", RpcTarget.MasterClient, viewId);
        }
        
        [PunRPC]
        public void Network_DestroyPhotonView(int viewId)
        {
            var pv = PhotonView.Find(viewId);
            if (pv != null)
            {
                PhotonNetwork.Destroy(pv);
            }
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
        
        public void UpdateForNewPlayer(Photon.Realtime.Player player)
        {
            // Send current player scores to new player.
            var playerName = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(playerName);
            this.photonView.RPC("Network_SetPlayerScore", player, playerName, currentScore);
        }
    }

}