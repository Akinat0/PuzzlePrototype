
using Abu.Tools.UI;
using UnityEngine;

public class ShardsPurchaseView : UIComponent
{
    [SerializeField] TextComponent text;

    TextComponent Text => text;

    public static ShardsPurchaseView Create(RectTransform container, ShardsPurchase purchase)
    {
        ShardsPurchaseView purchaseView = Instantiate(GetPrefab(purchase.Rarity), container);
        purchaseView.Text.Text = $"{EmojiHelper.GetShardEmojiText(purchase.Rarity)}{purchase.Cost.ToString()}";
        return purchaseView;
    }

    static ShardsPurchaseView GetPrefab(Rarity rarity)
    {
        string path;

        switch (rarity)
        {
            case Rarity.Common:
                path = "UI/ShardsCommonPurchaseView";
                break;
            case Rarity.Rare:
                path = "UI/ShardsRarePurchaseView";
                break;
            case Rarity.Epic:
                path = "UI/ShardsEpicPurchaseView";
                break;
            default:
                path = string.Empty;
                break;
        }

        return Resources.Load<ShardsPurchaseView>(path);
    }
}
