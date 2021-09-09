
using System;
using Abu.Tools;

public class FreeHeartBoosterTier : Tier
{
    const string Id = "free_heart_booster";
    
    public FreeHeartBoosterTier()
    {
        IsFreeTrigger = new Trigger($"{Id}_is_free", true);
        Reward = new BoosterReward(3, Account.GetBooster<HeartBooster>());
        FreePurchase = new FreePurchase(IsFreeTrigger.Value, new VideoPurchase());

        IsFreeTrigger.Changed += isFree => FreePurchase.IsFree = isFree;
    }

    public override string ID => Id;
    public override Reward Reward { get; }
    public override Purchase Purchase => FreePurchase;
    public override TierType Type => TierType.Booster;

    FreePurchase FreePurchase { get; set; }

    public override bool Obtain(Action onSuccess = null)
    {
        bool obtained = base.Obtain(onSuccess);
        
        //only first heart booster free
        if (obtained)
            IsFreeTrigger.Value = false;

        return obtained;
    }

    Trigger IsFreeTrigger { get; } 

    public override void Parse(TierInfo tierInfo)
    {
        throw new System.NotImplementedException();
    }
}
