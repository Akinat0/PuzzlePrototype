using UnityEngine;

public class FreezeBoosterAchievement : Achievement
{
    public FreezeBoosterAchievement()
    {
        Account.GetBooster<TimeFreezeBooster>().BoosterUsedEvent += BoosterUsedEvent_Handler;
    }
    
    readonly CoinsReward reward = new CoinsReward(400);
    
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
