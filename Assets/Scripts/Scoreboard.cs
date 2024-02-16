using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFirstARGame
{
    public class Scoreboard : MonoBehaviour
    {
        private Dictionary<string, int> scores;

        // Start is called before the first frame update
        void Start()
        {
            this.scores = new Dictionary<string, int>();
        }

        public void SetScore(string playerName, int score)
        {
            // Check to see if player exists on scoreboard.
            if (this.scores.ContainsKey(playerName)) {
                // Set the score for respective key.
                this.scores[playerName] = score;
            } else {
                this.scores.Add(playerName, score);
            }
        }

        public int GetScore(string playerName)
        {
            if (this.scores.ContainsKey(playerName)) {
                return this.scores[playerName];
            } else {
                // Player does not exist? -1 = Error.
                return -1;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            foreach (var score in this.scores) {
                GUILayout.Label($"{score.Key}: {score.Value}", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 22});
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
