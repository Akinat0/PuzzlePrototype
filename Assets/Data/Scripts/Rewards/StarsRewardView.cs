
using Abu.Tools.UI;
using TMPro;
using UnityEngine;

public class StarsRewardView : UIComponent
{
    static StarsRewardView prefab;
    static StarsRewardView Prefab
    {
        get
        {
            if (prefab == null)
                prefab = Resources.Load<StarsRewardView>("UI/StarsRewardView");

            return prefab;
        }
    } 
    
    public static StarsRewardView Create(RectTransform container, StarsReward starsReward)
    {
        StarsRewardView rewardView = Instantiate(Prefab, container);
        rewardView.StarsReward = starsReward;
        return rewardView;
    }

    StarsReward starsReward;

    StarsReward StarsReward
    {
        set
        {
            starsReward = value;
            TextField.text = starsReward.Amount.ToString();
        }
    }

    TextMeshProUGUI textField;

    TextMeshProUGUI TextField
    {
        get
        {
            if (textField == null)
                textField = GetComponentInChildren<TextMeshProUGUI>();

            return textField;
        }
    }
}
