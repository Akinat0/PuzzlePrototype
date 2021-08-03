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

        void Failed() =>
            ErrorWindow.Create(null, ErrorWindow.ErrorType.InternetConnection);
        
        void Skipped() =>
            ErrorWindow.Create(null, ErrorWindow.ErrorType.AdSkipped);
        
        videoPlacement = new RewardedVideoPlacement(success, Skipped, Failed);
        videoPlacement.Show();

        return true;
    }

    public override GameObject CreateView(RectTransform rectTransform)
    {
        return VideoPurchaseView.Create(rectTransform, this).gameObject;
    }
}
