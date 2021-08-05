using Abu.Tools.UI;
using Data.Scripts.ScreensScripts;
using ScreensScripts;

namespace Puzzle
{
    public class MainMenuShopScreenState : MainMenuUIState
    {
        ButtonComponent CloseButton { get; }
        ShopScreen ShopScreen { get; }

        public MainMenuShopScreenState(
            MainMenuUIManager mainMenu,
            ButtonComponent closeButton,
            ShopScreen shopScreen)
            : base(mainMenu)
        {
            CloseButton = closeButton;
            ShopScreen = shopScreen;
        }
        
        public override void Start()
        {
            LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs());
            
            CloseButton.OnClick += OnCloseButtonClick;
            ShopScreen.OnOverlayClick += OnCloseButtonClick;
            
            LauncherUI.ShowAchievementsScreenEvent += ShowAchievementScreenEvent_Handler;
            LauncherUI.ShowCollectionEvent += ShowCollectionEvent_Handler;
            LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
            
            CloseButton.ShowComponent();
            ShopScreen.Show();
        }

        public override void Stop()
        {
            CloseButton.OnClick -= OnCloseButtonClick;
            ShopScreen.OnOverlayClick -= OnCloseButtonClick;
            
            LauncherUI.ShowAchievementsScreenEvent -= ShowAchievementScreenEvent_Handler;
            LauncherUI.ShowCollectionEvent -= ShowCollectionEvent_Handler;
            LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
            
            CloseButton.HideComponent();
            ShopScreen.Hide();
        }

        void OnCloseButtonClick()
        {
            ChangeStateTo<MainMenuIdleScreenState>();
        }
        
        void ShowAchievementScreenEvent_Handler(Achievement achievement)
        {
            ChangeStateTo<MainMenuAchievementScreenState>();            
        }
        
        void ShowCollectionEvent_Handler(ShowCollectionEventArgs _)
        {
            ChangeStateTo<MainMenuIdleScreenState>();
        }
        
        void PlayLauncherEvent_Handler(PlayLauncherEventArgs _)
        {
            ChangeStateTo<MainMenuInGameState>();
        }
    }
}