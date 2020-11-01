using UnityEngine;

public class BoosterReward : Reward
{
    public int Amount { get; }
    public Booster Booster { get; }

    public BoosterReward(int amount, Booster booster)
    {
        Amount = amount;
        Booster = booster;
    }
    
    public override void CreateView(RectTransform container)
    {
        BoosterRewardView.Create(container, this);
    }

    public override void Claim()
    {
        switch (Booster)
        {
            case HeartBooster _:
                Account.GetBooster<HeartBooster>().Amount += Amount;
                break;
            case TimeFreezeBooster _:
                Account.GetBooster<TimeFreezeBooster>().Amount += Amount;
                break;
        }
        
    }
}
