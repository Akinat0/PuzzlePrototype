using System;
using UnityEngine;

public class TierWindow : CancelableWindow
{
    public static TierWindow Create(Tier tier, Action onSuccess = null, Action onCancel = null)
    {
        CancelableWindow prefab = Resources.Load<CancelableWindow>("UI/CancelableWindow");
        CancelableWindow cancelableWindow = Instantiate(prefab, Root);

        TierWindow window = ConvertTo<TierWindow>(cancelableWindow);
        
        window.Initialize(tier, onSuccess, onCancel);
        
        return window;
    }

    Tier Tier { get; set; }

    void Initialize(Tier tier, Action onSuccess, Action onCancel)
    {
        Tier = tier;
        
        Tier.Purchase.CreateView(OkButton.Content);

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
