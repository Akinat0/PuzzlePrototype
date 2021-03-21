using Abu.Tools.UI;
using UnityEngine;
using Puzzle;
using TMPro;

namespace ScreensScripts
{
    public class ReplayScreenManager : ManagerView
    {
        [SerializeField] GameObject ReplayScreen;
        [SerializeField] TextButtonComponent ReviveButton;
        [SerializeField] TextButtonComponent ReplayButton;
        [SerializeField] ButtonComponent MenuButton;
        [SerializeField] ButtonComponent PauseButton;
        [SerializeField] TextComponent TimerField;

        readonly VideoPurchase video = new VideoPurchase();
        
        void Start()
        {
            ReplayScreen.SetActive(false);
            ReviveButton.OnClick += OnReviveClick;
            ReplayButton.OnClick += OnReplayClick;
            MenuButton.OnClick += OnMenuClick;
        }
        
        void OnReplayClick()
        {
            Overlay.ChangePhase(0, 0.5f);
            GameSceneManager.Instance.InvokeResetLevel();
            ReplayScreen.SetActive(false);
            PauseButton.SetActive(true);
        }

        void OnReviveClick()
        {
            video.Process(Revive);
        }

        void Revive()
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
        void OnMenuClick()
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