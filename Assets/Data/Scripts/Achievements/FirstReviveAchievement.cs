using Puzzle;

public sealed class FirstReviveAchievement : Achievement
{
    public FirstReviveAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        GameSceneManager.PlayerReviveEvent += PlayerReviveEvent_Handler;
    }
    
    readonly StarsReward reward = new StarsReward(250);
    
    public override string Name => "New life";
    public override string Description => "Use Revive";
    public override Reward Reward => reward;
    public override float Goal => 1;

    void PlayerReviveEvent_Handler()
    {
        Progress++;
    }

}
