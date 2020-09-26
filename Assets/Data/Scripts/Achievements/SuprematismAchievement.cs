
using Puzzle;

public class SuprematismAchievement : Achievement
{
    public SuprematismAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;

        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
    }

    readonly CoinsReward reward = new CoinsReward(230);

    public override string Name => "Suprematism";
    public override string Description => "Complete suprematism";
    public override Reward Reward => reward;
    public override float Goal => 1;


    void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
    {
        if (args.LevelConfig.Name == "Suprem")
            Progress = Goal;
    }
}
