public sealed class HeartBoosterTier : Tier
{
    public HeartBoosterTier()
    {
        Available = Purchase.Available;
        Account.BalanceChangedEvent += OnBalanceChangedEvent_Handler;
    }
    
    Purchase purchase = new CoinsPurchase(50);
    Reward reward = new BoosterReward(2, Account.GetBooster<HeartBooster>());
    
    public override int ID => 1;
    public override Reward Reward => reward;
    public override Purchase Purchase => purchase;
    public override TierType Type => TierType.Booster;

    public override void Parse(TierInfo tierInfo)
    {
        if(tierInfo == null)
            return;
        
        purchase = new CoinsPurchase(tierInfo.Cost);
        reward = new BoosterReward(tierInfo.Amount, Account.GetBooster<HeartBooster>());
        InvokeTierValueChanged();
    }
    
    void OnBalanceChangedEvent_Handler(int balance)
    {
        Available = Purchase.Available;
    }
}
