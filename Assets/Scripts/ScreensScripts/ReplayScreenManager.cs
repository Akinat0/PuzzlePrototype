using System;
using UnityEngine;
using UnityEngine.UI;
using Abu.Tools;

namespace ScreensScripts
{
    
    public class ReplayScreenManager : MonoBehaviour
    {
        
        [SerializeField] 
        private AsyncLoader _asyncLoader;
        [SerializeField]
        private Text _endGameText;

        void Start()
        {
            int score = PlayerPrefs.GetInt("score", 0);
            _endGameText.text += " " + score;
        }

        public void Replay()
        {
            _asyncLoader.LoadScene("GameScene");
        }
    }
}