using Abu.Tools.UI;
using UnityEngine;

public class VideoPurchaseView : UIComponent
{
    static VideoPurchaseView prefab;
    static VideoPurchaseView Prefab
    {
        get
        {
            if (prefab == null)
                prefab = Resources.Load<VideoPurchaseView>("UI/VideoPurchaseView");

            return prefab;
        }
    }
    
    public static VideoPurchaseView Create(RectTransform container, VideoPurchase purchase)
    {
        VideoPurchaseView purchaseView = Instantiate(Prefab, container);
        return purchaseView;
    }
}
