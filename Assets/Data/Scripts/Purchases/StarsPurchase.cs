
using System;
using UnityEngine;

public class StarsPurchase : Purchase
{
    public StarsPurchase(int cost)
    {
        Cost = cost;
    }

    public int Cost { get; set; }

    public override bool Available => Account.HasStars(Cost);

    public override bool Process(Action success)
    {
        if (!Available)
            return false;

        if (Account.HasStars(Cost))
        {
            success?.Invoke();
            return true;
        }
        
        return false;
    }

    public override GameObject CreateView(RectTransform rectTransform)
    {
        return StarsPurchaseView.Create(rectTransform, this).gameObject;
    }
}
