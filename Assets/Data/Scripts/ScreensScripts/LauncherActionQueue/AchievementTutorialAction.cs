using Abu.Tools.UI;
using Data.Scripts.ScreensScripts;
using ScreensScripts;
using UnityEngine;

public class AchievementTutorialAction : TutorialAction
{
    [TutorialProperty("achievement_notification")]
    public AchievementNotification AchievementNotification { get; set; }
    
    [TutorialProperty("achievements_screen")]
    public AchievementsScreen AchievementScreen { get; set; }
    
    RectTransformTutorialHole tutorialHole;
    TutorialOverlayView tutorialOverlay;

    public override void Start()
    {
        tutorialOverlay = OverlayView.Create<TutorialOverlayView>(AchievementNotification.transform.parent,
            AchievementNotification.transform.GetSiblingIndex(), OverlayView.RaycastTargetMode.Never);

        tutorialOverlay.Color = new Color(0, 0, 0, 0.35f);

        tutorialHole = new RectTransformTutorialHole(AchievementNotification.Content);
        
        tutorialOverlay.AddHole(tutorialHole);
        tutorialOverlay.ChangePhase(0.975f, 0.5f);

        bool needPause = true;
        
        void OnAchievementShown(AchievementNotification achievementNotification)
        {
            achievementNotification.OnShown -= OnAchievementShown;
            if(needPause)
                TimeManager.Pause();
        }

        void OnAchievementClicked()
        {
            AchievementNotification.OnClick -= OnAchievementClicked;

            needPause = false;
            
            TimeManager.Unpause();
            tutorialOverlay.RemoveHole(tutorialHole);
            tutorialHole = null;
            
            LauncherUI.SelectLevel(Account.LevelConfigs[1]);

            StartAchievementScreenTutorial();
        }

        AchievementNotification.Setup(Account.GetAchievement<TutorialAchievement>());
        AchievementNotification.Show(null);
        AchievementNotification.OnShown += OnAchievementShown;
        AchievementNotification.OnClick += OnAchievementClicked;
    }

    void StartAchievementScreenTutorial()
    {
        AchievementScreen.IsScrollable = false;
        AchievementViewComponent achievementView = 
            AchievementScreen.GetAchievementView(Account.GetAchievement<TutorialAchievement>());
        
        tutorialOverlay.Phase = 0;
        tutorialHole = new RectTransformTutorialHole(achievementView.Content);
        tutorialOverlay.AddHole(tutorialHole);
        tutorialOverlay.ChangePhase(0.975f, 0.5f);

        void OnAchievementClick()
        {
            AchievementScreen.IsScrollable = true;
            tutorialOverlay.ChangePhase(0, 0.5f,() => tutorialOverlay.Destroy());
            achievementView.OnClick -= OnAchievementClick;
            Pop();
        }

        achievementView.OnClick += OnAchievementClick;
    }

    public override void Update()
    {
        base.Update();

        tutorialHole?.UpdateRect();
    }
}
