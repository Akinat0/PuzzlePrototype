
using Abu.Tools.UI;
using UnityEngine;

public class ChestReward : Reward
{
    int Amount { get; }
    Rarity Rarity { get; }
    
    public ChestReward(Rarity rarity, int amount)
    {
        Amount = amount;
        Rarity = rarity;
    }

    public override UIComponent CreateView(RectTransform container)
    {
        return ChestRewardView.Create(container, Rarity, Amount);
    }

    public override void Claim()
    {
        Account.GetChest(Rarity).Add(Amount);
    }
}
