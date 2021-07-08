using System;
using Abu.Tools.UI;
using UnityEngine;
using Puzzle;

namespace ScreensScripts
{
    public class ReplayScreen : ScreenComponent
    {
        [SerializeField] TextButtonComponent ReviveButton;
        [SerializeField] TextButtonComponent ReplayButton;
        [SerializeField] ButtonComponent MenuButton;

        readonly VideoPurchase video = new VideoPurchase();

        void Start()
        {
            ReviveButton.HideComponent(0);
            ReplayButton.HideComponent(0);
            MenuButton.HideComponent(0);

            ReviveButton.OnClick += OnReviveClick;
            ReplayButton.OnClick += OnReplayClick;
            MenuButton.OnClick += OnMenuClick;
        }

        public override bool Show(Action finished = null)
        {
            if (!base.Show(finished))
                return false;

            if(!GameSceneManager.Instance.Session.ReviveUsed)
                ReviveButton.ShowComponent();
            
            ReplayButton.ShowComponent();
            MenuButton.ShowComponent();

            return true;
        }
        
        public override bool Hide(Action finished = null)
        {
            if (!base.Hide(finished))
                return false;

            ReviveButton.HideComponent();
            ReplayButton.HideComponent();
            MenuButton.HideComponent();

            return true;
        }
        
        void OnEnable()
        {
            GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
        }

        void OnDisable()
        {
            GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
        }

        void OnReplayClick()
        {
            GameSceneManager.Instance.InvokeResetLevel();
            Hide();
        }

        void OnReviveClick()
        {
            video.Process(Revive);
        }

        void Revive()
        {
            Hide();
            GameSceneManager.Instance.InvokePlayerRevive();
        }
        
        void OnMenuClick()
        {
            Hide();
            GameSceneManager.Instance.InvokeLevelClosed();
        }

        void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
        {
            levelColorScheme.SetButtonColor(ReplayButton);
            levelColorScheme.SetButtonColor(ReviveButton);
            levelColorScheme.SetButtonColor(MenuButton);
        }
    }
}