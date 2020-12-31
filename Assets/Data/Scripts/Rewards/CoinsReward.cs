
using UnityEngine;

public class CoinsReward : Reward
{
    public int Amount { get; }
    
    public CoinsReward(int amount)
    {
        Amount = amount;
    }

    public override GameObject CreateView(RectTransform container)
    {
        return CoinsRewardView.Create(container, this).gameObject;
    }

    public override void Claim()
    {
        Account.AddCoins(Amount);
    }
}
