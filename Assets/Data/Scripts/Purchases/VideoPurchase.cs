using System;
using Puzzle.Advertisements;
using UnityEngine;

public class VideoPurchase : Purchase
{
    RewardedVideoPlacement videoPlacement;
    
    public override bool Available => true;

    public override bool Process(Action success)
    {
        if (!Available)
            return false;
        
        videoPlacement = new RewardedVideoPlacement(success, null, null);
        videoPlacement.Show();

        return true;
    }

    public override GameObject CreateView(RectTransform rectTransform)
    {
        return VideoPurchaseView.Create(rectTransform, this).gameObject;
    }
}
