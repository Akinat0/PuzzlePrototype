using UnityEngine;
using UnityEngine.UI;
using Puzzle;

namespace ScreensScripts
{
    public class ReplayScreenManager : ManagerView
    {
        [SerializeField] private GameObject replayScreen;
        [SerializeField] private Button reviveButton;
        [SerializeField] private Button replayButton;
        [SerializeField] private Button menuButton;
        

        private void Start()
        {
            replayScreen.SetActive(false);
        }

        public void Replay()
        {
            GameSceneManager.Instance.InvokeResetLevel();
            replayScreen.SetActive(false);
        }
        
        public void Revive()
        {
            GameSceneManager.Instance.InvokePlayerRevive();
            replayScreen.SetActive(false);
        }

        public void ToMenu()
        {
            GameSceneManager.Instance.InvokeLevelClosed();
            replayScreen.SetActive(false);
        }

        public void CreateReplyScreen()
        {
            replayScreen.SetActive(true);
        }
        
        protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
        {
            levelColorScheme.SetButtonColor(replayButton);
            levelColorScheme.SetButtonColor(reviveButton);
            levelColorScheme.SetButtonColor(menuButton);
        }
    }
}