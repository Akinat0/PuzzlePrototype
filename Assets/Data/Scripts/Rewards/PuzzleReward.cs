
using UnityEngine;

public class PuzzleReward : Reward
{
    public readonly int PuzzleID;
    
    public PuzzleReward(int puzzleID)
    {
        PuzzleID = puzzleID;
    }
    
    public override GameObject CreateView(RectTransform container)
    {
        return PuzzleRewardView.Create(container, this).gameObject;
    }

    public override void Claim()
    {
        Account.UnlockCollectionItem(PuzzleID);
    }
}
