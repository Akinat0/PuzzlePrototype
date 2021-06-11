
using Puzzle;

public sealed class TutorialAchievement : Achievement
{

    public TutorialAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;

        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
    }
    
    readonly StarsReward reward = new StarsReward(100);
    
    public override string Name => "Start Learning";
    public override string Description => "Complete tutorial";
    public override Reward Reward => reward;
    public override float Goal => 1;

    void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
    {
        if (args.LevelConfig.Name == "Tutorial")
            Progress = Goal;
    }
    
}
