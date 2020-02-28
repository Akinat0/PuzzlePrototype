using System;
using Puzzle;
using UnityEngine;


    public class CompleteScreenManager : MonoBehaviour
    {
        public static event Action CompleteLevelEvent;
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Complete()
        {
            CompleteLevelEvent?.Invoke();
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
