
public sealed class TimeFreezeBoosterTier : Tier
{
    public TimeFreezeBoosterTier()
    {
        Available = Purchase.Available;
    }

    Purchase purchase = new StarsPurchase(75);
    
    public override int ID => 4;
    public override Reward Reward => new BoosterReward(1, Account.GetBooster<TimeFreezeBooster>());
    public override Purchase Purchase => purchase;
    public override TierType Type => TierType.Booster;
    
    public override void Parse(TierInfo tierInfo)
    {
        if(tierInfo == null)
            return;
        
        purchase = new StarsPurchase(tierInfo.Cost);
        InvokeTierValueChanged();
    }
}
