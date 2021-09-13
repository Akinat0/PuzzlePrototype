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

        public AchievementsScreen Achievements => AchievementScreen;
        public CollectionScreen Collection => CollectionScreen;
        public ShopScreen Shop => ShopScreen;

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
            
            ManageButton(Account.AchievementsAvailable, achievementButton);
            ManageButton(Account.CollectionAvailable, collectionButton);
            ManageButton(Account.ShopAvailable, shopButton);

            Account.AchievementsAvailable.Changed += AchievementsAvailableChanged_Handler;
            Account.CollectionAvailable.Changed += CollectionsAvailableChanged_Handler;
            Account.ShopAvailable.Changed += ShopAvailableChanged_Handler;
        }

        void OnDestroy()
        {
            DisposeStates();
            
            Account.AchievementsAvailable.Changed -= AchievementsAvailableChanged_Handler;
            Account.CollectionAvailable.Changed -= CollectionsAvailableChanged_Handler;
            Account.ShopAvailable.Changed -= ShopAvailableChanged_Handler;
        }
        
        void OnEnable()
        {
            LauncherUI.ShowCollectionEvent += ShowCollectionEvent_Handler;
            LauncherUI.CloseCollectionEvent += CloseCollectionEvent_Handler;
            LauncherUI.LevelChangedEvent += LevelChangedEvent_Handler;
            LauncherUI.GameEnvironmentUnloadedEvent += GameEnvironmentUnloadedEvent_Handler;
            
            Account.CollectionAvailable.Changed += CollectionAvailableChanged_Handler;
        }

        void OnDisable()
        {
            LauncherUI.ShowCollectionEvent -= ShowCollectionEvent_Handler;
            LauncherUI.CloseCollectionEvent -= CloseCollectionEvent_Handler;
            LauncherUI.LevelChangedEvent -= LevelChangedEvent_Handler;
            LauncherUI.GameEnvironmentUnloadedEvent -= GameEnvironmentUnloadedEvent_Handler;
            
            Account.CollectionAvailable.Changed -= CollectionAvailableChanged_Handler;
        }

        void ManageButton(bool available, ButtonComponent button) => button.SetActive(available);

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

        void GameEnvironmentUnloadedEvent_Handler(GameSceneUnloadedArgs args)
        {
            if (args.LevelConfig == Account.LevelConfigs[1] && !Account.ShopAvailable)
                LauncherUI.Instance.ActionQueue.AddAction(new ShopTutorialAction(ShopScreen, shopButton, closeButton));
        }
        
        void CollectionAvailableChanged_Handler(bool available)
        {
            if (!available)
                return;

            bool CanStartPredicate() => CurrentState.GetType() == typeof(MainMenuIdleScreenState);
            
            LauncherUI.Instance.ActionQueue.AddAction(new CollectionTutorialAction(collectionButton, CollectionScreen, CanStartPredicate));
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

        void CollectionsAvailableChanged_Handler(bool available)
            => ManageButton(available, collectionButton);
        
        void AchievementsAvailableChanged_Handler(bool available)
            => ManageButton(available, achievementButton);
        
        void ShopAvailableChanged_Handler(bool available)
            => ManageButton(available, shopButton);
    }
}