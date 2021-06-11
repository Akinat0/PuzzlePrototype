using UnityEngine;

public sealed class FreezeBoosterAchievement : Achievement
{
    public FreezeBoosterAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        Account.GetBooster<TimeFreezeBooster>().BoosterUsedEvent += BoosterUsedEvent_Handler;
    }
    
    readonly StarsReward reward = new StarsReward(400);
    
    public override string Name => "Freeze booster";
    public override string Description => "Use Freeze Time Booster 5 times";
    public override Reward Reward => reward;
    public override float Goal => 5;

    public override Sprite Icon => Resources.Load<Sprite>("Achievements/FreezeBoosterAchievement");

    void BoosterUsedEvent_Handler()
    {
        Progress++;
    }
}
