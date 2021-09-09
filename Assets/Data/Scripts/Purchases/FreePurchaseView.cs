using Abu.Tools.UI;
using UnityEngine;

public class FreePurchaseView : UIComponent
{
    [SerializeField] RectTransform realPurchaseRoot;
    [SerializeField] RectTransform freePurchaseRoot;

    FreePurchase FreePurchase { get; set; }

    public static FreePurchaseView Create(RectTransform container, FreePurchase purchase)
    {
        FreePurchaseView purchaseView = Instantiate(Resources.Load<FreePurchaseView>("UI/FreePurchaseView"), container);
        purchaseView.Create(purchase);
        return purchaseView;
    }

    void Create(FreePurchase purchase)
    {
        FreePurchase = purchase;
        FreePurchase.IsFreeChanged += IsFreeChanged;
        FreePurchase.RealPurchase.CreateView(realPurchaseRoot);
        IsFreeChanged(FreePurchase.IsFree);
    }

    void IsFreeChanged(bool isFree)
    {
        freePurchaseRoot.gameObject.SetActive(isFree);
        realPurchaseRoot.gameObject.SetActive(!isFree);
    }

    void OnDestroy()
    {
        FreePurchase.IsFreeChanged -= IsFreeChanged;
    }
}
