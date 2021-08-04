using System;
using Abu.Tools;
using Abu.Tools.UI;
using Data.Scripts.ScreensScripts;
using DG.Tweening;
using Puzzle.Analytics;
using ScreensScripts;
using UnityEngine;

namespace Puzzle
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] RectTransform root; 
        [SerializeField] StarsComponent stars;
        [SerializeField] RectTransform miniButtonsContainer;
        [SerializeField] ButtonComponent collectionButton;
        [SerializeField] ButtonComponent achievementButton;
        [SerializeField] ButtonComponent shopButton;
        [SerializeField] ButtonComponent closeButton;

        [SerializeField] ShardsWalletComponent[] shardsWallets;

        [SerializeField] AchievementsScreen AchievementScreen;
        [SerializeField] ScreenComponent CollectionScreen;
        [SerializeField] ScreenComponent ShopScreen;

        public RectTransform Root => root;
        public StarsComponent Stars => stars;

        void Start()
        {
            AchievementScreen.CreateContent();
            AchievementScreen.OnOverlayClick += HideAllScreens;
            CollectionScreen.OnOverlayClick += HideAllScreens;
            ShopScreen.OnOverlayClick += HideAllScreens;
        }
        
        void OnDestroy()
        {
            AchievementScreen.OnOverlayClick -= HideAllScreens;
            CollectionScreen.OnOverlayClick -= HideAllScreens;
            ShopScreen.OnOverlayClick -= HideAllScreens;
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
            foreach (ShardsWalletComponent wallet in shardsWallets)
                wallet.Hide();
            
            stars.Show();

            closeButton.HideComponent(0.2f);
            AchievementScreen.Hide();
            CollectionScreen.Hide();
            ShopScreen.Hide();
        }
        
        void HideAllScreens(Action finished)
        {
            foreach (ShardsWalletComponent wallet in shardsWallets)
                wallet.Hide();
            
            stars.Show();
            
            DelegateGroup complete = new DelegateGroup(4, finished);

            closeButton.HideComponent(0.2f, complete);
            AchievementScreen.Hide(complete);
            CollectionScreen.Hide(complete);
            ShopScreen.Hide(complete);
        }

        void ShopButtonOnClick()
        {
            closeButton.ShowComponent();
            ShopScreen.Show();
            
            new SimpleAnalyticsEvent("mini_shop_button_clicked").Send();
        }
        
        void AchievementButtonOnClick()
        {
            closeButton.ShowComponent();
            AchievementScreen.Show();
            
            new SimpleAnalyticsEvent("mini_achievement_button_clicked").Send();
        }
        
        void CollectionButtonOnClick()
        {
            foreach (ShardsWalletComponent wallet in shardsWallets)
                wallet.Show();
            
            closeButton.ShowComponent();
            CollectionScreen.Show();

            new SimpleAnalyticsEvent("mini_collection_button_clicked").Send();
        }

        void CloseButtonClick()
        {
            HideAllScreens();
        }
        
        void OnEnable()
        {
            LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
            LauncherUI.GameEnvironmentUnloadedEvent += GameEnvironmentUnloadedEventHandler;
            LauncherUI.ShowCollectionEvent += ShowCollectionEvent_Handler;
            LauncherUI.CloseCollectionEvent += CloseCollectionEvent_Handler;
            LauncherUI.LevelChangedEvent += LevelChangedEvent_Handler;
            LauncherUI.ShowAchievementsScreenEvent += ShowAchievementScreenEvent_Handler;
            
            collectionButton.OnClick += CollectionButtonOnClick;
            achievementButton.OnClick += AchievementButtonOnClick;
            shopButton.OnClick += ShopButtonOnClick;
            closeButton.OnClick += CloseButtonClick;
        }

        void OnDisable()
        {
            LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
            LauncherUI.GameEnvironmentUnloadedEvent -= GameEnvironmentUnloadedEventHandler;
            LauncherUI.ShowCollectionEvent -= ShowCollectionEvent_Handler;
            LauncherUI.CloseCollectionEvent -= CloseCollectionEvent_Handler;
            LauncherUI.LevelChangedEvent -= LevelChangedEvent_Handler;
            LauncherUI.ShowAchievementsScreenEvent -= ShowAchievementScreenEvent_Handler;
            
            collectionButton.OnClick -= CollectionButtonOnClick;
            achievementButton.OnClick -= AchievementButtonOnClick;
            shopButton.OnClick -= ShopButtonOnClick;
            closeButton.OnClick -= CloseButtonClick;
        }
        
        void ShowCollectionEvent_Handler(ShowCollectionEventArgs _)
        {
            HideAllScreens();
            
            foreach (ShardsWalletComponent wallet in shardsWallets)
                wallet.Show();
        }
        
        void CloseCollectionEvent_Handler(CloseCollectionEventArgs _)
        {
            foreach (ShardsWalletComponent wallet in shardsWallets)
                wallet.Hide();
        }
        
        void PlayLauncherEvent_Handler(PlayLauncherEventArgs _ )
        {
            HideMiniButtons();
            Stars.Alpha = 0;
        }

        void GameEnvironmentUnloadedEventHandler(GameSceneUnloadedArgs _)
        {
            ShowMiniButtons();
            Stars.Alpha = 1;
        }

        void LevelChangedEvent_Handler(LevelChangedEventArgs args)
        {
            args.LevelConfig.ColorScheme.SetButtonColor(closeButton);
        }

        void ShowAchievementScreenEvent_Handler(Achievement achievement)
        {
            void ShowAchievements()
            {
                closeButton.ShowComponent();
                AchievementScreen.Show();
            }
            
            if (!AchievementScreen.Shown)
                HideAllScreens(ShowAchievements);
            
            if(achievement != null)
                AchievementScreen.FocusOn(achievement);
        }
    }
}