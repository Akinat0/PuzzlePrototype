using System;
using Abu.Tools.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BoosterRewardView : UIComponent
{
    static BoosterRewardView prefab;
    static BoosterRewardView Prefab
    {
        get
        {
            if (prefab == null)
                prefab = Resources.Load<BoosterRewardView>("UI/BoosterRewardView");

            return prefab;
        }
    } 
    
    public static BoosterRewardView Create(RectTransform container, BoosterReward boosterReward)
    {
        BoosterRewardView rewardView = Instantiate(Prefab, container);
        rewardView.BoosterReward = boosterReward;
        return rewardView;
    }

    BoosterReward boosterReward;
    
    protected BoosterReward BoosterReward
    {
        set
        {
            boosterReward = value;
            TextField.text = boosterReward.Amount.ToString();

            string path =
                $"Textures/{boosterReward.Booster.Name.Replace(" ", String.Empty).ToLowerInvariant()}_booster_image";

            Image.sprite = Resources.Load<Sprite>(path);
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
    
    protected Image image;
    
    protected Image Image
    {
        get
        {
            if (image == null)
                image = GetComponentInChildren<Image>();

            return image;
        }
    }
}
