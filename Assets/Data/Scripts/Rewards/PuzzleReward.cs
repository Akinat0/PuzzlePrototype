
using UnityEngine;

public class PuzzleReward : Reward
{
    public readonly int PuzzleID;
    
    public PuzzleReward(int puzzleID)
    {
        PuzzleID = puzzleID;
    }
    
    public override void CreateView(RectTransform container)
    {
        PuzzleRewardView.Create(container, this);
    }

    public override void Claim()
    {
        Account.UnlockCollectionItem(PuzzleID);
    }
}
