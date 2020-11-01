
using UnityEngine;

public class CoinsPurchase : Purchase
{
    public CoinsPurchase(int cost)
    {
        Cost = cost;
    }

    public int Cost { get; private set; }

    public override bool Available => Account.Coins >= Cost;

    public override bool Process()
    {
        if (!Available)
            return false;
        
        return Account.RemoveCoins(Cost);;
    }

    public override void CreateView(RectTransform rectTransform)
    {
        CoinsPurchaseView.Create(rectTransform, this);
    }
}
