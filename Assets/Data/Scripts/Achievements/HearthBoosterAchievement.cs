using UnityEngine;

public sealed class HearthBoosterAchievement : Achievement
{
    public HearthBoosterAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        Account.GetBooster<HeartBooster>().BoosterUsedEvent += BoosterUsedEvent_Handler;
    }
    
    readonly CoinsReward reward = new CoinsReward(400);
    
    public override string Name => "Hearth booster";
    public override string Description => "Use Hearth Booster 5 times";
    public override Reward Reward => reward;
    public override float Goal => 5;

    public override Sprite Icon => Resources.Load<Sprite>("Achievements/FreezeBoosterAchievement");

    void BoosterUsedEvent_Handler()
    {
        Progress++;
    }
}
