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
            CurrentState = states[typeof(MainMenuIdleScreenState)];
            CurrentState.Start();
        }

        void Start()
        {
            AchievementScreen.CreateContent();
        }

        void OnDestroy()
        {
            DisposeStates();
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
    }
}