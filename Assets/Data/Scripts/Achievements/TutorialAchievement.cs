using Puzzle;

public sealed class TutorialAchievement : Achievement
{

    public TutorialAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;

        GameSceneManager.LevelStartedEvent += LevelStartedEvent_Handler;
    }

    public override string Name => "Start Learning";
    public override string Description => "Complete tutorial";
    public override Reward Reward { get; } = new ChestReward(Rarity.Common, 1);

    public override float Goal => 1;

    void LevelStartedEvent_Handler()
    {
        if (GameSceneManager.Instance.LevelConfig.Name == "Tutorial")
            Progress = Goal;
    }
    
}
