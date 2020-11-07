
public sealed class TimeFreezeBoosterTier : Tier
{
    public TimeFreezeBoosterTier()
    {
        Available = Purchase.Available;
        Account.BalanceChangedEvent += OnBalanceChangedEvent_Handler;
    }

    Purchase purchase = new CoinsPurchase(75);
    
    public override int ID => 4;
    public override Reward Reward => new BoosterReward(1, Account.GetBooster<TimeFreezeBooster>());
    public override Purchase Purchase => purchase;
    public override TierType Type => TierType.Booster;
    
    public override void Parse(TierInfo tierInfo)
    {
        if(tierInfo == null)
            return;
        
        purchase = new CoinsPurchase(tierInfo.Cost);
        InvokeTierValueChanged();
    }

    void OnBalanceChangedEvent_Handler(int balance)
    {
        Available = Purchase.Available;
    }
}
