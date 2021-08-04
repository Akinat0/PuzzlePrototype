using Abu.Tools.UI;
using UnityEngine;

public class StarsPurchaseView : UIComponent
{
    [SerializeField] TextComponent text;

    TextComponent Text => text;

    public static StarsPurchaseView Create(RectTransform container, StarsPurchase purchase)
    {
        StarsPurchaseView purchaseView = Instantiate(Resources.Load<StarsPurchaseView>("UI/StarsPurchaseView"), container);
        purchaseView.Text.Text = $"{EmojiHelper.StarEmoji}{purchase.Cost.ToString()}";
        return purchaseView;
    }
    
    
}