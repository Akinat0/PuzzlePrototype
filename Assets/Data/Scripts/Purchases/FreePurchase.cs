
using System;
using UnityEngine;

public class FreePurchase : Purchase
{
    public FreePurchase(bool isFree, Purchase realPurchase)
    {
        IsFree = isFree;
        RealPurchase = realPurchase;
    }

    public Action<bool> IsFreeChanged;
    public Purchase RealPurchase { get; }

    public bool IsFree
    {
        get => isFree;
        set
        {
            if (isFree == value)
                return;

            isFree = value;
            IsFreeChanged?.Invoke(isFree);
        }
    }
    
    public override bool Available => IsFree || RealPurchase.Available;

    bool isFree;

    public override bool Process(Action success)
    {
        if (IsFree)
        {
            success?.Invoke();
            return true;
        }
        
        return RealPurchase.Process(success);
    }

    public override GameObject CreateView(RectTransform rectTransform)
    {
        return FreePurchaseView.Create(rectTransform, this).gameObject;
    }
}
