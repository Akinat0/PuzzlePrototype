
using Puzzle;

public class RitualAchievement : Achievement
{
    public RitualAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;

        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
    }

    readonly CoinsReward reward = new CoinsReward(230);

    public override string Name => "Ritual";
    public override string Description => "Complete ritual";
    public override Reward Reward => reward;
    public override float Goal => 1;


    void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
    {
        if (args.LevelConfig.Name == "Ritual")
            Progress = Goal;
    }
}
