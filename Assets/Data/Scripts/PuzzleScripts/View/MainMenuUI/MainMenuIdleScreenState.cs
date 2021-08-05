using Abu.Tools.UI;
using Puzzle.Analytics;
using ScreensScripts;

namespace Puzzle
{
    public class MainMenuIdleScreenState : MainMenuUIState
    {
        ButtonComponent CollectionButton { get; }
        ButtonComponent AchievementsButton { get; }
        ButtonComponent ShopButton { get; }

        ShardsWalletComponent[] ShardsWallets { get; }

        public MainMenuIdleScreenState(MainMenuUIManager mainMenu, ButtonComponent collectionButton, ButtonComponent achievementsButton, ButtonComponent shopButton, ShardsWalletComponent[] shardsWallets) : base(mainMenu)
        {
            CollectionButton = collectionButton;
            AchievementsButton = achievementsButton;
            ShopButton = shopButton;
            ShardsWallets = shardsWallets;
        }

        public override void Start()
        {
            LauncherUI.ShowAchievementsScreenEvent += ShowAchievementScreenEvent_Handler;
            LauncherUI.PlayLauncherEvent           += PlayLauncherEvent_Handler;

            CollectionButton.OnClick   += OnCollectionClick;
            AchievementsButton.OnClick += OnAchievementClick;
            ShopButton.OnClick         += OnShopClick;
        }

        public override void Stop()
        {
            LauncherUI.ShowAchievementsScreenEvent -= ShowAchievementScreenEvent_Handler;
            LauncherUI.PlayLauncherEvent           -= PlayLauncherEvent_Handler;

            CollectionButton.OnClick   -= OnCollectionClick;
            AchievementsButton.OnClick -= OnAchievementClick;
            ShopButton.OnClick         -= OnShopClick;
        }

        void OnCollectionClick()
        {
            new SimpleAnalyticsEvent("mini_shop_button_clicked").Send();
            ChangeStateTo<MainMenuCollectionScreenState>();
        }
        
        void OnAchievementClick()
        {
            new SimpleAnalyticsEvent("mini_achievement_button_clicked").Send();
            ChangeStateTo<MainMenuAchievementScreenState>();
        }
        
        void OnShopClick()
        {
            new SimpleAnalyticsEvent("mini_collection_button_clicked").Send();
            ChangeStateTo<MainMenuShopScreenState>();
        }
        
        void ShowAchievementScreenEvent_Handler(Achievement achievement)
        {
            ChangeStateTo<MainMenuAchievementScreenState>();            
        }

        void PlayLauncherEvent_Handler(PlayLauncherEventArgs _)
        {
            ChangeStateTo<MainMenuInGameState>();
        }
    }
}