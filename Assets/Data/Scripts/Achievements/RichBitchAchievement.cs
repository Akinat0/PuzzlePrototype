
public sealed class RichBitchAchievement : Achievement
{
    public RichBitchAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        Account.StarsAmountChanged += StarsAmountChangedEvent_Handler;
    }

    public override string Name => "Rich Bitch";
    public override string Description => "Collect 10 stars";
    public override Reward Reward { get; } = new ChestReward(Rarity.Common, 1);

    public override float Goal => 1000;
    
    void StarsAmountChangedEvent_Handler(int balance)
    {
        Progress = balance;
    }
    
}
