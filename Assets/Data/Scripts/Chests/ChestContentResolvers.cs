
public class CommonChestContentResolver : IChestContentResolver
{
    public Reward[] GetRewards()
    {
        return new Reward[]
        {
            new BoosterReward(2, Account.GetBooster<HeartBooster>())
        };
    }
}

public class RareChestContentResolver : IChestContentResolver
{
    public Reward[] GetRewards()
    {
        return new Reward[]
        {
            new BoosterReward(3, Account.GetBooster<HeartBooster>()),
            new BoosterReward(1, Account.GetBooster<TimeFreezeBooster>())
        };
    }
}

public class EpicChestContentResolver : IChestContentResolver
{
    public Reward[] GetRewards()
    {
        return new Reward[]
        {
            new BoosterReward(10, Account.GetBooster<HeartBooster>()),
            new BoosterReward(5, Account.GetBooster<TimeFreezeBooster>())
        };
    }
}
