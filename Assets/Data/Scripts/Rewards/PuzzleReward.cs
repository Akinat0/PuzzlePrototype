
using Abu.Tools.UI;
using UnityEngine;

public class PuzzleReward : Reward
{
    public readonly int PuzzleID;
    public override Rarity Rarity { get; }

    public PuzzleReward(int puzzleID)
    {
        PuzzleID = puzzleID;
        Rarity = Account.GetCollectionItem(puzzleID).Rarity;
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
