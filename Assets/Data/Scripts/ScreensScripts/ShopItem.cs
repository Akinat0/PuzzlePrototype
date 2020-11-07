using Abu.Tools.UI;
using UnityEngine;

public class ShopItem : UIComponent
{
    [SerializeField] int TierID;

    [SerializeField] RectTransform RewardContainer;
    [SerializeField] RectTransform PurchaseContainer;
    [SerializeField] ButtonComponent Button;
    
    Tier Tier;

    GameObject PurchaseView;
    GameObject RewardView;
    
    void Start()
    {
        Tier = Account.GetTier(TierID);

        if (Tier == null)
        {
            Debug.LogError($"Tier {TierID} doesn't exist");
            return;
        }
        
        CreateView();
        
        Button.OnClick += () => Tier.Obtain();
        Tier.OnAvailableChangedEvent += OnAvailableChangedEvent_Handler;
        Tier.OnTierValueChangedEvent += OnTierValueChangedEvent_Handler;
    }


    void CreateView()
    {
        Button.Interactable = Tier.Available;
        PurchaseView = Tier.Purchase.CreateView(PurchaseContainer);
        RewardView = Tier.Reward.CreateView(RewardContainer); 
    }

    void OnAvailableChangedEvent_Handler(bool available)
    {
        Button.Interactable = available;
    }

    void OnTierValueChangedEvent_Handler()
    {
        Destroy(PurchaseView);
        Destroy(RewardView);
        
        CreateView();
    }

}
