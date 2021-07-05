using Abu.Tools.UI;
using UnityEngine;

public class BoosterReward : Reward
{
    public override Rarity Rarity => Booster.Rarity;

    public int Amount { get; }
    public Booster Booster { get; }

    public BoosterReward(int amount, Booster booster)
    {
        Amount = amount;
        Booster = booster;
    }
    
    public override UIComponent CreateView(RectTransform container)
    {
        return BoosterRewardView.Create(container, this);
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
