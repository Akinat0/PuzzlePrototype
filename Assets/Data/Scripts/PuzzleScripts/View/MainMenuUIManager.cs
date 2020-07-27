using System;
using Abu.Tools.UI;
using DG.Tweening;
using ScreensScripts;
using UnityEngine;

namespace Puzzle
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] WalletComponent wallet;
        [SerializeField] RectTransform miniButtonsContainer;
        [SerializeField] ButtonComponent collectionButton;
        [SerializeField] ButtonComponent achievementButton;
        [SerializeField] ButtonComponent levelsButton;
        
        public WalletComponent Wallet => wallet;
        public ButtonComponent CollectionButton => collectionButton;
        public ButtonComponent AchievementButton => achievementButton;
        public ButtonComponent LevelsButton => levelsButton;

        void HideMiniButtons()
        {
            miniButtonsContainer.DOAnchorPosY(-600, LevelSelectorComponent.UiAnimationDuration);
        }

        void ShowMiniButtons()
        {
            miniButtonsContainer.DOAnchorPosY(0, LevelSelectorComponent.UiAnimationDuration).SetDelay(0.25f);
        }
        
        void OnEnable()
        {
            LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
            LauncherUI.GameSceneUnloadedEvent += GameSceneUnloadedEvent_Handler;
            CollectionButton.OnClick += CollectionButtonOnClick;
        }

        void OnDisable()
        {
            LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
            LauncherUI.GameSceneUnloadedEvent -= GameSceneUnloadedEvent_Handler;
            CollectionButton.OnClick -= CollectionButtonOnClick;
        }
        
        void CollectionButtonOnClick()
        {
            LauncherUI.Instance.InvokeOpenCollectionMenu();
        }
        
        void PlayLauncherEvent_Handler(PlayLauncherEventArgs _ )
        {
            HideMiniButtons();
            Wallet.Alpha = 0;
        }

        void GameSceneUnloadedEvent_Handler()
        {
            ShowMiniButtons();
            Wallet.Alpha = 1;
        }
    }
}