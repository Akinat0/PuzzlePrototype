using Abu.Tools.UI;
using UnityEngine;
using Puzzle;
using TMPro;

namespace ScreensScripts
{
    public class ReplayScreenManager : ManagerView
    {
        [SerializeField] private GameObject ReplayScreen;
        [SerializeField] private TextButtonComponent ReviveButton;
        [SerializeField] private TextButtonComponent ReplayButton;
        [SerializeField] private ButtonComponent MenuButton;
        [SerializeField] private ButtonComponent PauseButton;
        [SerializeField] private TextMeshProUGUI TimerField;

        private void Start()
        {
            ReplayScreen.SetActive(false);
            ReviveButton.OnClick += OnReviveClick;
            ReplayButton.OnClick += OnReplayClick;
            MenuButton.OnClick += OnMenuClick;
        }
        
        private void OnReplayClick()
        {
            Overlay.ChangePhase(0, 0.5f);
            GameSceneManager.Instance.InvokeResetLevel();
            ReplayScreen.SetActive(false);
            PauseButton.SetActive(true);
        }

        private void OnReviveClick()
        {
            Overlay.ChangePhase(0, 0.5f);
            
            if (TimerField != null)
            {
                ReplayScreen.SetActive(false);
                StartCoroutine(CountdownRoutine(TimerField, () =>
                {
                    TimerField.gameObject.SetActive(false);
                    GameSceneManager.Instance.InvokePlayerRevive();
                    PauseButton.SetActive(true);
                }));
            }
            else
            {
                ReplayScreen.SetActive(false);
                GameSceneManager.Instance.InvokePlayerRevive();
            }
        }

        private void OnMenuClick()
        {
            Overlay.ChangePhase(0, 0.5f);
            GameSceneManager.Instance.InvokeLevelClosed();
            ReplayScreen.SetActive(false);
        }

        public void CreateReplyScreen(bool reviveUsed)
        {
            ReplayScreen.SetActive(true);
            PauseButton.SetActive(false);
            ReviveButton.SetActive(!reviveUsed);
            
            Overlay.ChangePhase(1, 0.5f);
        }
        
        protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
        {
            levelColorScheme.SetButtonColor(ReplayButton);
            levelColorScheme.SetButtonColor(ReviveButton);
            levelColorScheme.SetButtonColor(MenuButton);
        }
    }
}