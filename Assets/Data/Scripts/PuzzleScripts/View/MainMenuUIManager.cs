using System;
using Abu.Tools.UI;
using DG.Tweening;
using ScreensScripts;
using UnityEngine;

namespace Puzzle
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] RectTransform root; 
        [SerializeField] WalletComponent wallet;
        [SerializeField] RectTransform miniButtonsContainer;
        [SerializeField] ButtonComponent collectionButton;
        [SerializeField] ButtonComponent achievementButton;
        [SerializeField] ButtonComponent shopButton;
        [SerializeField] ButtonComponent closeButton;

        [SerializeField] GameObject AchievementScreen;
        [SerializeField] GameObject CollectionScreen;
        [SerializeField] GameObject ShopScreen;

        public RectTransform Root => root;
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
            ShopScreen.SetActive(false);
        }

        void ShopButtonOnClick()
        {
            HideAllScreens();
            closeButton.SetActive(true);
            ShopScreen.SetActive(true);
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
            LauncherUI.GameEnvironmentUnloadedEvent += GameEnvironmentUnloadedEventHandler;
            LauncherUI.ShowCollectionEvent += ShowCollectionEvent_Handler;
            LauncherUI.LevelChangedEvent += LevelChangedEvent_Handler;
            collectionButton.OnClick += CollectionButtonOnClick;
            achievementButton.OnClick += AchievementButtonOnClick;
            shopButton.OnClick += ShopButtonOnClick;
            closeButton.OnClick += CloseButtonOnClick;
        }

        void OnDisable()
        {
            LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
            LauncherUI.GameEnvironmentUnloadedEvent -= GameEnvironmentUnloadedEventHandler;
            LauncherUI.ShowCollectionEvent -= ShowCollectionEvent_Handler;
            collectionButton.OnClick -= CollectionButtonOnClick;
            achievementButton.OnClick -= AchievementButtonOnClick;
            shopButton.OnClick -= ShopButtonOnClick;
            closeButton.OnClick -= CloseButtonOnClick;
        }
        
        void ShowCollectionEvent_Handler(ShowCollectionEventArgs _)
        {
            HideAllScreens();
        }
        void PlayLauncherEvent_Handler(PlayLauncherEventArgs _ )
        {
            HideMiniButtons();
            Wallet.Alpha = 0;
        }

        void GameEnvironmentUnloadedEventHandler(GameSceneUnloadedArgs _)
        {
            ShowMiniButtons();
            Wallet.Alpha = 1;
        }

        void LevelChangedEvent_Handler(LevelChangedEventArgs args)
        {
            closeButton.Color = args.LevelConfig.ColorScheme.ButtonColor;
        }
    }
}