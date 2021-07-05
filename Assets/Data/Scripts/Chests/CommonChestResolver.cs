
public class CommonChestResolver : IChestContentResolver
{
    public Reward[] GetRewards()
    {
        return new Reward[]
        {
            new BoosterReward(1, Account.GetBooster<HeartBooster>()),
            new BoosterReward(1, Account.GetBooster<TimeFreezeBooster>())
        };
    }
}
