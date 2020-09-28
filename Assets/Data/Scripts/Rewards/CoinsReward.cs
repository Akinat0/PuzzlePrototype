
using UnityEngine;

public class CoinsReward : Reward
{
    public int Amount { get; }
    
    public CoinsReward(int amount)
    {
        Amount = amount;
    }

    public override void CreateView(RectTransform container)
    {
        CoinsRewardView.Create(container, this);
    }

    public override void Claim()
    {
        Account.AddCoins(Amount);
    }
}
