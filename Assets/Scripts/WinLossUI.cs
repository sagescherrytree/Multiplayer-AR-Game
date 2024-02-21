using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFirstARGame
{
    public class WinLossUI : MonoBehaviour
    {
        [SerializeField]
        GameObject winText;

        [SerializeField]
        GameObject lossText;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void showText(bool win)
        {
            if (win)
            {
                winText.SetActive(true);
            }
            else
            {
                lossText.SetActive(true);
            }
        }
    }
}
