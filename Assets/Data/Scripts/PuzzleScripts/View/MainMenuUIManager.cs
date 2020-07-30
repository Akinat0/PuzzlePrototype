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
        [SerializeField] ButtonComponent closeButton;

        [SerializeField] GameObject AchievementScreen;
        [SerializeField] GameObject CollectionScreen;
        
        public WalletComponent Wallet => wallet;

        void Awake()
        {
            HideAllScreens();
        }

        void HideMiniButtons()
        {
            miniButtonsContainer.DOAnchorPosY(-600, LevelSelectorComponent.UiAnimationDuration);
        }

        void ShowMiniButtons()
        {
            miniButtonsContainer.DOAnchorPosY(0, LevelSelectorComponent.UiAnimationDuration).SetDelay(0.25f);
        }

        void HideAllScreens()
        {
            closeButton.SetActive(false);
            AchievementScreen.SetActive(false);
            CollectionScreen.SetActive(false);
        }

        void AchievementButtonOnClick()
        {
            HideAllScreens();
            closeButton.SetActive(true);
            AchievementScreen.SetActive(true);
        }
        
        void CollectionButtonOnClick()
        {
            HideAllScreens();
            closeButton.SetActive(true);
            CollectionScreen.SetActive(true);
        }

        void CloseButtonOnClick()
        {
            HideAllScreens();
        }
        
        void OnEnable()
        {
            LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
            LauncherUI.GameSceneUnloadedEvent += GameSceneUnloadedEvent_Handler;
            collectionButton.OnClick += CollectionButtonOnClick;
            achievementButton.OnClick += AchievementButtonOnClick;
            closeButton.OnClick += CloseButtonOnClick;
        }

        void OnDisable()
        {
            LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
            LauncherUI.GameSceneUnloadedEvent -= GameSceneUnloadedEvent_Handler;
            collectionButton.OnClick -= CollectionButtonOnClick;
            achievementButton.OnClick -= AchievementButtonOnClick;
            closeButton.OnClick -= CloseButtonOnClick;
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