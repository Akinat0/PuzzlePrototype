using UnityEngine;

public sealed class FreezeBoosterAchievement : Achievement
{
    public FreezeBoosterAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        Account.GetBooster<TimeFreezeBooster>().BoosterUsedEvent += BoosterUsedEvent_Handler;
    }

    public override string Name => "Freeze booster";
    public override string Description => "Use Freeze Time Booster 5 times";
    public override Reward Reward { get; } = new ChestReward(Rarity.Rare, 1);

    public override float Goal => 5;

    public override Sprite Icon => Resources.Load<Sprite>("Achievements/FreezeBoosterAchievement");

    void BoosterUsedEvent_Handler()
    {
        Progress++;
    }
}
