using Abu.Tools.UI;
using Data.Scripts.ScreensScripts;
using ScreensScripts;
using UnityEngine;

public class AchievementTutorialAction : LauncherAction
{
    AchievementNotification AchievementNotification { get; }
    AchievementsScreen AchievementScreen { get; }
    
    RectTransformTutorialHole tutorialHole;
    TutorialOverlayView tutorialOverlay;

    public AchievementTutorialAction(AchievementsScreen achievementsScreen, AchievementNotification achievementNotification) : base(LauncherActionOrder.Tutorial)
    {
        AchievementNotification = achievementNotification;
        AchievementScreen = achievementsScreen;
    }

    public override void Start()
    {
        if (Account.AchievementsAvailable)
        {
            Pop();
            return;
        }

        Account.AchievementsAvailable.Value = true;

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

            Account.CollectionAvailable.Value = true;
            
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
