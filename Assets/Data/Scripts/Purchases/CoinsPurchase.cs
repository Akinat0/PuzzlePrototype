
using System;
using UnityEngine;

public class CoinsPurchase : Purchase
{
    public CoinsPurchase(int cost)
    {
        Cost = cost;
    }

    public int Cost { get; set; }

    public override bool Available => Account.Coins >= Cost;

    public override bool Process(Action success)
    {
        if (!Available)
            return false;

        if (Account.RemoveCoins(Cost))
        {
            success?.Invoke();
            return true;
        }
        
        return false;
    }

    public override GameObject CreateView(RectTransform rectTransform)
    {
        return CoinsPurchaseView.Create(rectTransform, this).gameObject;
    }
}
