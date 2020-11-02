public sealed class HeartBoosterTier : Tier
{
    public HeartBoosterTier()
    {
        Available = Purchase.Available;
        Account.BalanceChangedEvent += OnBalanceChangedEvent_Handler;
    }
    
    public override int ID => 1;
    public override Reward Reward => new BoosterReward(2, Account.GetBooster<HeartBooster>());
    public override Purchase Purchase => new CoinsPurchase(50);

    void OnBalanceChangedEvent_Handler(int balance)
    {
        Available = Purchase.Available;
    }
}
