
using Abu.Tools.UI;
using UnityEngine;

public class PuzzleReward : Reward
{
    public readonly int PuzzleID;
    
    public PuzzleReward(int puzzleID)
    {
        PuzzleID = puzzleID;
    }
    
    public override UIComponent CreateView(RectTransform container)
    {
        return PuzzleRewardView.Create(container, this);
    }

    public override void Claim()
    {
        Account.UnlockCollectionItem(PuzzleID);
    }
}
