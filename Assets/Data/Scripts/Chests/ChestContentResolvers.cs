using UnityEngine;

public class CommonChestContentResolver : IChestContentResolver
{
    public Reward[] GetRewards()
    {
        return new Reward[]
        {
            new BoosterReward(Random.Range(1, 3), Account.GetBooster<HeartBooster>()),
            new ShardsReward(Random.Range(5, 10), Rarity.Common)
        };
    }
}

public class RareChestContentResolver : IChestContentResolver
{
    public Reward[] GetRewards()
    {
        return new Reward[]
        {
            new BoosterReward(Random.Range(3,6), Account.GetBooster<HeartBooster>()),
            new ShardsReward(Random.Range(10, 20), Rarity.Common),
            new ShardsReward(Random.Range(5, 10), Rarity.Rare)
        };
    }
}

public class EpicChestContentResolver : IChestContentResolver
{
    public Reward[] GetRewards()
    {
        return new Reward[]
        {
            new BoosterReward(Random.Range(4,8), Account.GetBooster<HeartBooster>()),
            new BoosterReward(Random.Range(1,3), Account.GetBooster<TimeFreezeBooster>()),
            new ShardsReward(Random.Range(20, 40), Rarity.Common),
            new ShardsReward(Random.Range(10, 20), Rarity.Rare),
            new ShardsReward(Random.Range(5, 10), Rarity.Epic)
        };
    }
}
