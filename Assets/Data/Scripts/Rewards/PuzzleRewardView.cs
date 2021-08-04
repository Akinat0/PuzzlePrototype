using Abu.Tools.UI;
using UnityEngine;

public class PuzzleRewardView : UIComponent
{
    public static PuzzleRewardView Create(RectTransform container, PuzzleReward puzzleReward)
    {
        PuzzleRewardView rewardView = Instantiate(Resources.Load<PuzzleRewardView>("UI/PuzzleRewardView"), container);
        
        rewardView.Create(puzzleReward);
        
        return rewardView;
    }

    [SerializeField] RectTransform PuzzleContainer;
    [SerializeField] TextComponent Text;


    void Create(PuzzleReward reward)
    {
        Color textFieldColor;

        switch (reward.Rarity)
        {
            case Rarity.Common:
                textFieldColor = new Color(0.679f, 0.679f, 0.679f);
                break;
            case Rarity.Rare:
                textFieldColor = new Color(0.287f, 0.843f, 1f);
                break;
            case Rarity.Epic:
                textFieldColor = new Color(0.988f, 0.485f, 1f);
                break;
            default:
                textFieldColor = Color.gray;
                break;
        }

        Text.Color = textFieldColor;
        Text.Text = Account.GetCollectionItem(reward.PuzzleID).Name;;
        UIPuzzleView.Create(reward.PuzzleID, PuzzleContainer);
    }
}