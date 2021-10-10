
public class LauncherAchievementAction : LauncherAction
{
    public LauncherAchievementAction(Achievement achievement, AchievementNotification achievementNotification) 
        : base(LauncherActionOrder.Achievement)
    {
        Achievement = achievement;
        AchievementNotification = achievementNotification;
    }

    Achievement Achievement { get; }
    AchievementNotification AchievementNotification { get; }


    public override void Start()
    {
        AchievementNotification.Setup(Achievement);
        AchievementNotification.Show(Pop);
    }
}
