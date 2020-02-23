using Puzzle;
using UnityEngine;


    public class CompleteScreenManager : MonoBehaviour
    {
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
