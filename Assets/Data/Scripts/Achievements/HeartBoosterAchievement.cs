
public class HeartBoosterAchievement : Achievement
{
    public HeartBoosterAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        Account.GetBooster<HeartBooster>().BoosterUsedEvent += BoosterUsedEvent_Handler;
    }
    
    public override string Name => "Heart Booster";
    public override string Description => "Use additional heart 10 times";
    public override Reward Reward => new ChestReward(Rarity.Common, 2);
    public override float Goal => 10;

    void BoosterUsedEvent_Handler()
    {
        Progress++;
    }
}
