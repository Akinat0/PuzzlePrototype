
using Puzzle;

public sealed class TutorialAchievement : Achievement
{

    public TutorialAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;

        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
    }

    public override string Name => "Start Learning";
    public override string Description => "Complete tutorial";
    public override Reward Reward { get; } = new ChestReward(Rarity.Epic, 1);

    public override float Goal => 1;

    void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
    {
        if (args.LevelConfig.Name == "Tutorial")
            Progress = Goal;
    }
    
}
