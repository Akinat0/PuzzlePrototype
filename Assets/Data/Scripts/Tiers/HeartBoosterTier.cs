
public class HeartBoosterTier : Tier
{
    public override int ID => 1;
    public override Reward Reward => new BoosterReward(2, Account.GetBooster<HeartBooster>());
    public override Purchase Purchase => new CoinsPurchase(50);
}
