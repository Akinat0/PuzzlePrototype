using System;
using System.Collections.Generic;
using Abu.Tools.UI;
using Data.Scripts.ScreensScripts;
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
        [SerializeField] CollectionScreen CollectionScreen;
        [SerializeField] ShopScreen ShopScreen;


        public RectTransform Root => root;
        public StarsComponent Stars => stars;

        public MainMenuUIState CurrentState { get; private set; }

        Dictionary<Type, MainMenuUIState> states;
        

        public void ChangeStateTo<TState>() where TState : MainMenuUIState
        {
            Debug.Log($"[MainMenu] Change state from {CurrentState.GetType()} to {typeof(TState)}");
            
            CurrentState.Stop();
            CurrentState = states[typeof(TState)];
            CurrentState.Start();
        }

        void Awake()
        {
            CreateStates();
            RegisterTutorialValues();
            CurrentState = states[typeof(MainMenuIdleScreenState)];
            CurrentState.Start();
        }

        void Start()
        {    
            AchievementScreen.CreateContent();
            
            ManageButton(Tutorials.AchievementTutorial.State, achievementButton);
            ManageButton(Tutorials.CollectionTutorial.State, collectionButton);
            ManageButton(Tutorials.ShopTutorial.State, shopButton);

            Tutorials.AchievementTutorial.StateChanged += AchievementsAvailableChanged_Handler;
            Tutorials.CollectionTutorial.StateChanged += CollectionsAvailableChanged_Handler;
            Tutorials.ShopTutorial.StateChanged += ShopAvailableChanged_Handler;
        }

        void OnDestroy()
        {
            DisposeStates();
            
            Tutorials.AchievementTutorial.StateChanged -= AchievementsAvailableChanged_Handler;
            Tutorials.CollectionTutorial.StateChanged -= CollectionsAvailableChanged_Handler;
            Tutorials.ShopTutorial.StateChanged -= ShopAvailableChanged_Handler;
        }
        
        void OnEnable()
        {
            LauncherUI.ShowCollectionEvent += ShowCollectionEvent_Handler;
            LauncherUI.CloseCollectionEvent += CloseCollectionEvent_Handler;
            LauncherUI.LevelChangedEvent += LevelChangedEvent_Handler;
        }

        void OnDisable()
        {
            LauncherUI.ShowCollectionEvent -= ShowCollectionEvent_Handler;
            LauncherUI.CloseCollectionEvent -= CloseCollectionEvent_Handler;
            LauncherUI.LevelChangedEvent -= LevelChangedEvent_Handler;
        }

        void ManageButton(TutorialState state, ButtonComponent button) 
            => button.SetActive(state == TutorialState.Started || state == TutorialState.Completed);

        void CreateStates()
        {
            states = new Dictionary<Type, MainMenuUIState>
            {
                {
                    typeof(MainMenuIdleScreenState),
                    new MainMenuIdleScreenState(this, collectionButton, achievementButton, shopButton, shardsWallets)
                },
                {
                    typeof(MainMenuCollectionScreenState),
                    new MainMenuCollectionScreenState(this, closeButton, CollectionScreen, shardsWallets)
                },
                {
                    typeof(MainMenuAchievementScreenState),
                    new MainMenuAchievementScreenState(this, closeButton, AchievementScreen)
                },
                {
                    typeof(MainMenuShopScreenState),
                    new MainMenuShopScreenState(this, closeButton, ShopScreen)
                },
                {
                    typeof(MainMenuInGameState),
                    new MainMenuInGameState(this, stars, miniButtonsContainer)
                }
            };
        }

        void DisposeStates()
        {
            foreach (MainMenuUIState state in states.Values)
                state.Dispose();
        }

        void LevelChangedEvent_Handler(LevelChangedEventArgs args)
        {
            args.LevelConfig.ColorScheme.SetButtonColor(closeButton);
        }

        void ShowCollectionEvent_Handler(ShowCollectionEventArgs _)
        {
            foreach (ShardsWalletComponent wallet in shardsWallets)
                wallet.Show();
        }

        void CloseCollectionEvent_Handler(CloseCollectionEventArgs _)
        {
            foreach (ShardsWalletComponent wallet in shardsWallets)
                wallet.Hide();
        }

        void CollectionsAvailableChanged_Handler(TutorialState state)
            => ManageButton(state, collectionButton);
        
        void AchievementsAvailableChanged_Handler(TutorialState state)
            => ManageButton(state, achievementButton);
        
        void ShopAvailableChanged_Handler(TutorialState state)
            => ManageButton(state, shopButton);


        void RegisterTutorialValues()
        {
            Tutorials.Register("achievements_screen", AchievementScreen);
            Tutorials.Register("collection_screen", CollectionScreen);
            Tutorials.Register("shop_screen", ShopScreen);

            Tutorials.Register("achievements_button", achievementButton);
            Tutorials.Register("collection_button", collectionButton);
            Tutorials.Register("shop_button", shopButton);
            
            Tutorials.Register("main_menu_close_button", closeButton);
            
            bool CanStartCollectionTutorial() => CurrentState.GetType() == typeof(MainMenuIdleScreenState);
            
            Tutorials.Register("can_start_collection_tutorial",  (Func<bool>) CanStartCollectionTutorial );
            
        }
    }
}