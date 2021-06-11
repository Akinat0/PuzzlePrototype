
public sealed class RichBitchAchievement : Achievement
{
    public RichBitchAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        Account.StarsAmountChanged += StarsAmountChangedEvent_Handler;
    }
    
    readonly StarsReward reward = new StarsReward(10);
    
    public override string Name => "Rich Bitch";
    public override string Description => "Collect 10 stars";
    public override Reward Reward => reward;
    public override float Goal => 1000;
    
    void StarsAmountChangedEvent_Handler(int balance)
    {
        Progress = balance;
    }
    
}
