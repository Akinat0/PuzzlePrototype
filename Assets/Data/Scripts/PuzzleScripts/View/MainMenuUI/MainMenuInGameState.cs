using Abu.Tools.UI;
using DG.Tweening;
using ScreensScripts;
using UnityEngine;

namespace Puzzle
{
    public class MainMenuInGameState : MainMenuUIState
    {
        StarsComponent Stars { get; }
        RectTransform ButtonsContainer { get; }

        public MainMenuInGameState(MainMenuUIManager mainMenu, StarsComponent stars, RectTransform buttonsContainer) : base(mainMenu)
        {
            Stars = stars;
            ButtonsContainer = buttonsContainer;
        }

        public override void Start()
        {
            LauncherUI.GameEnvironmentUnloadedEvent += GameEnvironmentUnloadedEvent_Handler;
            
            HideMiniButtons();
            Stars.Hide();
        }

        public override void Stop()
        {
            LauncherUI.GameEnvironmentUnloadedEvent -= GameEnvironmentUnloadedEvent_Handler;
            
            ShowMiniButtons();
            Stars.Show();
        }
        
        void HideMiniButtons()
        {
            ButtonsContainer.DOKill();
            ButtonsContainer.DOAnchorPosY(-600, LevelSelectorComponent.UiAnimationDuration);
        }
        
        void ShowMiniButtons()
        {
            ButtonsContainer.DOKill();
            ButtonsContainer.DOAnchorPosY(0, LevelSelectorComponent.UiAnimationDuration).SetDelay(0.25f);
        }

        void GameEnvironmentUnloadedEvent_Handler(GameSceneUnloadedArgs _)
        {
            ChangeStateTo<MainMenuIdleScreenState>();
        }
    }
}