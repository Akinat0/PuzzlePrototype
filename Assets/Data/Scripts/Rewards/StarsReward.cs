
using UnityEngine;

public class StarsReward : Reward
{
    public int Amount { get; }
    
    public StarsReward(int amount)
    {
        Amount = amount;
    }

    public override GameObject CreateView(RectTransform container)
    {
        return StarsRewardView.Create(container, this).gameObject;
    }

    public override void Claim()
    {
        Account.AddStars(Amount);
    }
}
