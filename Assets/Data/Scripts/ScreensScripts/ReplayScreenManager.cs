using System;
using UnityEngine;
using UnityEngine.UI;
using Abu.Tools;
using Puzzle;

namespace ScreensScripts
{
    
    public class ReplayScreenManager : MonoBehaviour
    {
    
        [SerializeField] private Text endGameText;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Replay()
        {
            GameSceneManager.Instance.InvokeGameStarted();
            gameObject.SetActive(false);
        }

        public void CreateReplyScreen()
        {
            gameObject.SetActive(true);
            int score = PlayerPrefs.GetInt("score", 0);
            endGameText.text = "Score " + score;
        }
    }
}