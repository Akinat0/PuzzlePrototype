using Abu.Tools.UI;
using UnityEngine;

public class VideoPurchaseView : UIComponent
{
    public static VideoPurchaseView Create(RectTransform container, VideoPurchase purchase)
    {
        VideoPurchaseView purchaseView = Instantiate(Resources.Load<VideoPurchaseView>("UI/VideoPurchaseView"), container);
        return purchaseView;
    }
}
