using System;
using UnityEngine;

public class TierWindow : CancelableWindow
{
    public static TierWindow Create(int TierID, Action onSuccess = null, Action onCancel = null)
    {
        CancelableWindow prefab = Resources.Load<CancelableWindow>("UI/CancelableWindow");
        CancelableWindow cancelableWindow = Instantiate(prefab, Root);

        TierWindow window = ConvertTo<TierWindow>(cancelableWindow);
        
        window.Initialize(TierID, onSuccess, onCancel);
        
        return window;
    }

    protected Tier Tier;
    
    protected void Initialize(int TierID, Action onSuccess, Action onCancel)
    {
        Tier = Account.GetTier(TierID);

        Tier.Purchase.CreateView(OkButton.RectTransform);

        OkButton.Interactable = Tier.Available;
        
        void CreateContent(RectTransform container)
        {
            Tier.Reward.CreateView(container);
        }

        void OnSuccess()
        {
            if(Tier.Obtain())
                onSuccess?.Invoke();
            
            Hide();
        }

        void OnCancel()
        {
            onCancel?.Invoke();
            Hide();
        }
        
        Initialize(CreateContent, OnSuccess, OnCancel, "Unlock", string.Empty, "Cancel");
    }
    
    
    
}
