using Abu.Tools.UI;
using TMPro;
using UnityEngine;

public class PuzzleRewardView : UIComponent
{
    #region factory
    
    static PuzzleRewardView prefab;
    static PuzzleRewardView Prefab
    {
        get
        {
            if (prefab == null)
                prefab = Resources.Load<PuzzleRewardView>("UI/PuzzleRewardView");

            return prefab;
        }
    } 
    
    public static PuzzleRewardView Create(RectTransform container, PuzzleReward puzzleReward)
    {
        PuzzleRewardView rewardView = Instantiate(Prefab, container);
        rewardView.PuzzleReward = puzzleReward;
        return rewardView;
    }
    
    #endregion

    [SerializeField] RectTransform PuzzleContainer;
    [SerializeField] TextMeshProUGUI TextField;
    
    PuzzleReward puzzleReward;

    protected PuzzleReward PuzzleReward
    {
        set
        {
            puzzleReward = value;
            TextField.text = Account.GetCollectionItem(puzzleReward.PuzzleID).Name;
            UIPuzzleView.Create(puzzleReward.PuzzleID, PuzzleContainer);
        }
    }
}