
using Puzzle;

public class NeonPartyAchievement : Achievement
{
    public NeonPartyAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;

        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
    }

    readonly CoinsReward reward = new CoinsReward(500);

    public override string Name => "Neon Party";
    public override string Description => "Complete Neon Party";
    public override Reward Reward => reward;
    public override float Goal => 1;


    void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
    {
        if (args.LevelConfig.Name == "NeonParty")
            Progress = Goal;
    }
}
