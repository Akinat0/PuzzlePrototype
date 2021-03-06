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

        [SerializeField] AchievementsScreen AchievementScreen;
        [SerializeField] ScreenComponent CollectionScreen;
        [SerializeField] ScreenComponent ShopScreen;

        public RectTransform Root => root;
        public StarsComponent Stars => stars;

        void Start()
        {
            AchievementScreen.CreateContent();
        }

        void HideMiniButtons()
        {
            miniButtonsContainer.DOAnchorPosY(-600, LevelSelectorComponent.UiAnimationDuration);
        }

        void ShowMiniButtons()
        {
            miniButtonsContainer.DOAnchorPosY(0, LevelSelectorComponent.UiAnimationDuration).SetDelay(0.25f);
        }

        void HideAllScreens(Action finished = null)
        {
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
            
            collectionButton.OnClick -= CollectionButtonOnClick;
            achievementButton.OnClick -= AchievementButtonOnClick;
            shopButton.OnClick -= ShopButtonOnClick;
            closeButton.OnClick -= CloseButtonClick;
        }
        
        void ShowCollectionEvent_Handler(ShowCollectionEventArgs _)
        {
            HideAllScreens();
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