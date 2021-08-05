
using Abu.Tools.UI;
using UnityEngine;

public class ShardsReward : Reward
{
    public ShardsReward(int amount, Rarity rarity)
    {
        Rarity = rarity;
        Amount = amount;
    }
    
    public override Rarity Rarity { get; }
    public int Amount { get; }

    public override void Claim()
    {
        Account.GetShards(Rarity).Add(Amount);
    }
    
    public override UIComponent CreateView(RectTransform container)
    {
        return ShardsRewardView.Create(container, this);
    }
}
