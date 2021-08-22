using Abu.Tools.UI;
using UnityEngine;

public class AchievementTutorialAction : LauncherAction
{
    AchievementNotification AchievementNotification { get; }
    RectTransformTutorialHole tutorialHole;

    public AchievementTutorialAction(AchievementNotification achievementNotification) : base(LauncherActionOrder.Tutorial)
    {
        AchievementNotification = achievementNotification;
    }

    public override void Start()
    {
        if (Account.AchievementsAvailable)
        {
            Pop();
            return;
        }

        Account.AchievementsAvailable.Value = true;

        TutorialOverlayView tutorialOverlay = OverlayView.Create<TutorialOverlayView>(AchievementNotification.transform.parent,
            AchievementNotification.transform.GetSiblingIndex(), OverlayView.RaycastTargetMode.Never);

        tutorialHole = new RectTransformTutorialHole(AchievementNotification.Content);
        
        tutorialOverlay.AddHole(tutorialHole);
        
        tutorialOverlay.ChangePhase(0.5f, 0.5f);

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
            tutorialOverlay.ChangePhase(0, 0.5f, () => Object.DestroyImmediate(tutorialOverlay.gameObject));
            
            Pop();
        }

        AchievementNotification.Setup(Account.GetAchievement<TutorialAchievement>());
        AchievementNotification.Show(null);
        AchievementNotification.OnShown += OnAchievementShown;
        AchievementNotification.OnClick += OnAchievementClicked;
    }

    public override void Update()
    {
        base.Update();
        
        if(tutorialHole != null)
            tutorialHole.UpdateRect();
    }
}
