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
            GameSceneManager.Instance.InvokeResetLevel();
            gameObject.SetActive(false);
        }
        
        public void Revive()
        {
            GameSceneManager.Instance.InvokePlayerRevive();
            gameObject.SetActive(false);
        }

        public void ToMenu()
        {
            GameSceneManager.Instance.InvokeLevelClosed();
            gameObject.SetActive(false);
        }

        public void CreateReplyScreen()
        {
            gameObject.SetActive(true);
        }
    }
}