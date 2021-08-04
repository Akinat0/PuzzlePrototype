using System;
using UnityEngine;

public class ShardsPurchase : Purchase
{
    public ShardsPurchase(int cost, Rarity rarity)
    {
        Cost = cost;
        Rarity = rarity;
    }
    
    public int Cost { get; }
    public Rarity Rarity { get; }
    public override bool Available => Account.GetShards(Rarity).Has(Cost);
    public override bool Process(Action success)
    {
        if (!Available)
            return false;

        if (!Account.GetShards(Rarity).TryRemove(Cost))
            return false;
        
        success?.Invoke();
        return true;
    }

    public override GameObject CreateView(RectTransform container)
    {
        return ShardsPurchaseView.Create(container, this).gameObject;
    }
}
