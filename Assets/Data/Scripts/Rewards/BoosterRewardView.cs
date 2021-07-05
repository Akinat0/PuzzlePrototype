using Abu.Tools.UI;
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

            //TODO use badge system for amount
            
            string path =
                $"Textures/{boosterReward.Booster.Name.Replace(" ", string.Empty).ToLowerInvariant()}_booster_image";

            Image.sprite = Resources.Load<Sprite>(path);
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
