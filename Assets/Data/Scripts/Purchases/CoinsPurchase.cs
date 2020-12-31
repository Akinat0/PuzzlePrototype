
using UnityEngine;

public class CoinsPurchase : Purchase
{
    public CoinsPurchase(int cost)
    {
        Cost = cost;
    }

    public int Cost { get; set; }

    public override bool Available => Account.Coins >= Cost;

    public override bool Process()
    {
        if (!Available)
            return false;
        
        return Account.RemoveCoins(Cost);;
    }

    public override GameObject CreateView(RectTransform rectTransform)
    {
        return CoinsPurchaseView.Create(rectTransform, this).gameObject;
    }
}
