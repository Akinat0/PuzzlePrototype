
using Abu.Tools.UI;
using UnityEngine;

public class ShardsRewardView : TextComponent
{
    public static TextComponent Create(RectTransform container, ShardsReward shardsReward)
    {
        string text = $"{shardsReward.Amount}{EmojiHelper.GetShardEmojiText(shardsReward.Rarity)}";
        TextComponent rewardView = Create(container, text, "shards_reward");
        rewardView.Color = shardsReward.Rarity.GetColor();
        return rewardView;
    }

}
