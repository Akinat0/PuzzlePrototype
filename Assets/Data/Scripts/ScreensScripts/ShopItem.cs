using Abu.Tools.UI;
using UnityEngine;

public class ShopItem : UIComponent
{
    [SerializeField] int TierID;

    [SerializeField] RectTransform RewardContainer;
    [SerializeField] RectTransform PurchaseContainer;
    [SerializeField] ButtonComponent Button;
    
    Tier Tier;
    
    void Start()
    {
        Tier = Account.GetTier(TierID);

        if (Tier == null)
        {
            Debug.LogError($"Tier {TierID} doesn't exist");
            return;
        }
        
        Button.Interactable = Tier.Available;
        Button.OnClick += () => Tier.Obtain();

        Tier.OnAvailableChangedEvent += OnAvailableChangedEvent_Handler;

        Tier.Purchase.CreateView(PurchaseContainer);
        Tier.Reward.CreateView(RewardContainer);
    }

    void OnAvailableChangedEvent_Handler(bool available)
    {
        Button.Interactable = available;
    }

}
