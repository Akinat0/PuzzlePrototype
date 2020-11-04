
public sealed class TimeFreezeBoosterTier : Tier
{
    public TimeFreezeBoosterTier()
    {
        Available = Purchase.Available;
        Account.BalanceChangedEvent += OnBalanceChangedEvent_Handler;
    }

    public override int ID => 4;
    public override Reward Reward => new BoosterReward(1, Account.GetBooster<TimeFreezeBooster>());
    public override Purchase Purchase => new CoinsPurchase(75);
    public override TierType Type => TierType.Booster;

    void OnBalanceChangedEvent_Handler(int balance)
    {
        Available = Purchase.Available;
    }
}
