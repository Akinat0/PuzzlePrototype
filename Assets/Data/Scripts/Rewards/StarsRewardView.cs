using Abu.Tools.UI;
using UnityEngine;

public class StarsRewardView : UIComponent
{
    public static StarsRewardView Create(RectTransform container, StarsReward starsReward)
    {
        StarsRewardView rewardView = Instantiate(Resources.Load<StarsRewardView>("UI/StarsRewardView"), container);
        rewardView.Create(starsReward);
        return rewardView;
    }

    [SerializeField] TextComponent textField;
    
    void Create(StarsReward starsReward)
    {
        textField.Text = $"{starsReward.Amount}{EmojiHelper.StarEmoji}";
    }
}
