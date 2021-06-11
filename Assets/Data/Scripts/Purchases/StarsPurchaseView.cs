using Abu.Tools.UI;
using TMPro;
using UnityEngine;

public class StarsPurchaseView : UIComponent
{
    [SerializeField] TextComponent text;
    
    static StarsPurchaseView prefab;
    static StarsPurchaseView Prefab
    {
        get
        {
            if (prefab == null)
                prefab = Resources.Load<StarsPurchaseView>("UI/StarsPurchaseView");

            return prefab;
        }
    }

    public TextComponent Text => text;

    public static StarsPurchaseView Create(RectTransform container, StarsPurchase purchase)
    {
        StarsPurchaseView purchaseView = Instantiate(Prefab, container);
        purchaseView.Text.Text = purchase.Cost.ToString();
        return purchaseView;
    }
    
    
}