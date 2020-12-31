using Abu.Tools.UI;
using TMPro;
using UnityEngine;

public class CoinsPurchaseView : UIComponent
{
    [SerializeField] TextMeshProUGUI text;
    
    static CoinsPurchaseView prefab;
    static CoinsPurchaseView Prefab
    {
        get
        {
            if (prefab == null)
                prefab = Resources.Load<CoinsPurchaseView>("UI/CoinsPurchaseView");

            return prefab;
        }
    }

    public TextMeshProUGUI Text => text;

    public static CoinsPurchaseView Create(RectTransform container, CoinsPurchase purchase)
    {
        CoinsPurchaseView purchaseView = Instantiate(Prefab, container);
        purchaseView.Text.text = purchase.Cost.ToString();
        return purchaseView;
    }
    
    
}