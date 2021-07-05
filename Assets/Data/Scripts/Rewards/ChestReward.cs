
using Abu.Tools.UI;
using UnityEngine;

public class ChestReward : Reward
{
    public override Rarity Rarity { get; }
    int Amount { get; }
    
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
