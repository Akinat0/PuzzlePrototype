
using Abu.Tools.UI;
using TMPro;
using UnityEngine;

public class CoinsRewardView : UIComponent
{
    static CoinsRewardView prefab;
    static CoinsRewardView Prefab
    {
        get
        {
            if (prefab == null)
                prefab = Resources.Load<CoinsRewardView>("UI/CoinsRewardView");

            return prefab;
        }
    } 
    
    public static CoinsRewardView Create(RectTransform container, CoinsReward coinsReward)
    {
        CoinsRewardView rewardView = Instantiate(Prefab, container);
        rewardView.CoinsReward = coinsReward;
        return rewardView;
    }

    CoinsReward coinsReward;
    
    protected CoinsReward CoinsReward
    {
        set
        {
            coinsReward = value;
            TextField.text = coinsReward.Amount.ToString();
        }
    }

    protected TextMeshProUGUI textField;
    
    protected TextMeshProUGUI TextField
    {
        get
        {
            if (textField == null)
                textField = GetComponentInChildren<TextMeshProUGUI>();

            return textField;
        }
    }
}
