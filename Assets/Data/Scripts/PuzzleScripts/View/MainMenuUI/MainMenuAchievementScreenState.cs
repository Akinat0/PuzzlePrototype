using Abu.Tools.UI;
using Data.Scripts.ScreensScripts;
using ScreensScripts;

namespace Puzzle
{
    public class MainMenuAchievementScreenState : MainMenuUIState
    {
        ButtonComponent CloseButton { get; }
        AchievementsScreen AchievementsScreen { get; }

        Achievement AchievementToFocus { get; set; }

        public MainMenuAchievementScreenState(
            MainMenuUIManager mainMenu,
            ButtonComponent closeButton,
            AchievementsScreen achievementsScreen)
            : base(mainMenu)
        {
            CloseButton = closeButton;
            AchievementsScreen = achievementsScreen;
            
            LauncherUI.ShowAchievementsScreenEvent += ShowAchievementScreenEvent_Handler;
        }
        
        public override void Dispose()
        {
            LauncherUI.ShowAchievementsScreenEvent -= ShowAchievementScreenEvent_Handler;
        }
        
        public override void Start()
        {
            LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs());
            
            CloseButton.OnClick += OnCloseButtonClick; 
            AchievementsScreen.OnOverlayClick += OnCloseButtonClick;
            
            LauncherUI.ShowCollectionEvent += ShowCollectionEvent_Handler;
            LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;

            CloseButton.ShowComponent();
            AchievementsScreen.Show();

            if (AchievementToFocus != null)
            {
                AchievementsScreen.FocusOn(AchievementToFocus);
                AchievementToFocus = null;
            }
        }

        public override void Stop()
        {
            CloseButton.OnClick -= OnCloseButtonClick;
            AchievementsScreen.OnOverlayClick -= OnCloseButtonClick;

            LauncherUI.ShowCollectionEvent -= ShowCollectionEvent_Handler;
            LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
            
            CloseButton.HideComponent();
            AchievementsScreen.Hide();
        }

        void OnCloseButtonClick()
        {
            ChangeStateTo<MainMenuIdleScreenState>();
        }
        
        void ShowAchievementScreenEvent_Handler(Achievement achievement)
        {
            if (IsRunning)
            {
                if (achievement != null) AchievementsScreen.FocusOn(achievement);
            }
            else
            {
                AchievementToFocus = achievement;
            }
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