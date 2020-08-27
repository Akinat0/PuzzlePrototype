
public sealed class RichBitchAchievement : Achievement
{
    public RichBitchAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        Account.BalanceChangedEvent += BalanceChangedEvent_Handler;
    }
    
    readonly CoinsReward reward = new CoinsReward(1000);
    
    public override string Name => "Rich Bitch";
    public override string Description => "Collect 1000 coins";
    public override Reward Reward => reward;
    public override float Goal => 1000;
    
    void BalanceChangedEvent_Handler(int balance)
    {
        Progress = balance;
    }
    
}
