using Abu.Tools.UI;
using UnityEngine;

public class RewardShine : UIComponent
{
    public static RewardShine Create(RectTransform parent, Rarity rarity)
    {
        string path = $"UI/RewardShine/reward_shine_{rarity.ToString().ToLowerInvariant()}";
        RewardShine prefab = Resources.Load<RewardShine>(path);
        RewardShine rewardShine = Instantiate(prefab, parent);
        return rewardShine;
    }
}
