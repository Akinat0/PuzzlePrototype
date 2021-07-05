
using Abu.Tools.UI;
using UnityEngine;

public class StarsReward : Reward
{
    public int Amount { get; }
    
    public StarsReward(int amount)
    {
        Amount = amount;
    }

    public override Rarity Rarity => Rarity.None;

    public override UIComponent CreateView(RectTransform container)
    {
        return StarsRewardView.Create(container, this);
    }

    public override void Claim()
    {
        Account.AddStars(Amount);
    }
}
