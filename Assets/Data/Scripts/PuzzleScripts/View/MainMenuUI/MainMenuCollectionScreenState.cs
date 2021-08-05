using Abu.Tools.UI;
using Data.Scripts.ScreensScripts;
using ScreensScripts;

namespace Puzzle
{
    public class MainMenuCollectionScreenState : MainMenuUIState
    {
        ButtonComponent CloseButton { get; }
        CollectionScreen CollectionScreen { get; }
        ShardsWalletComponent[] ShardsWallets { get; }

        public MainMenuCollectionScreenState(
            MainMenuUIManager mainMenu,
            ButtonComponent closeButton,
            CollectionScreen collectionScreen,
            ShardsWalletComponent[] shardsWallets)
            : base(mainMenu)
        {
            CloseButton = closeButton;
            CollectionScreen = collectionScreen;
            ShardsWallets = shardsWallets;
        }
        
        public override void Start()
        {
            LauncherUI.ShowCollectionEvent += ShowCollectionEvent_Handler;
            LauncherUI.ShowAchievementsScreenEvent += ShowAchievementScreenEvent_Handler;
            LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
            
            CloseButton.OnClick += OnCloseButtonClick; 
            CollectionScreen.OnOverlayClick += OnCloseButtonClick;
            
            LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs());

            CloseButton.ShowComponent();
            CollectionScreen.Show();
            
            foreach (ShardsWalletComponent wallet in ShardsWallets)
                wallet.Show();
        }

        public override void Stop()
        {
            CloseButton.OnClick -= OnCloseButtonClick;
            CollectionScreen.OnOverlayClick -= OnCloseButtonClick;
            
            LauncherUI.ShowCollectionEvent -= ShowCollectionEvent_Handler;
            LauncherUI.ShowAchievementsScreenEvent -= ShowAchievementScreenEvent_Handler;
            LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
            
            CloseButton.HideComponent();
            CollectionScreen.Hide();
        }

        void OnCloseButtonClick()
        {
            foreach (ShardsWalletComponent wallet in ShardsWallets)
                wallet.Hide();
            
            ChangeStateTo<MainMenuIdleScreenState>();
        }
        
        void ShowAchievementScreenEvent_Handler(Achievement achievement)
        {
            foreach (ShardsWalletComponent wallet in ShardsWallets)
                wallet.Hide();
            
            ChangeStateTo<MainMenuAchievementScreenState>();            
        }

        void ShowCollectionEvent_Handler(ShowCollectionEventArgs _)
        {
            ChangeStateTo<MainMenuIdleScreenState>();
        }

        void PlayLauncherEvent_Handler(PlayLauncherEventArgs _)
        {
            foreach (ShardsWalletComponent wallet in ShardsWallets)
                wallet.Hide();
            
            ChangeStateTo<MainMenuInGameState>();
        }
    }
}