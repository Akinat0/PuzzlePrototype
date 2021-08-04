using System;
using System.Collections;
using Abu.Tools.UI;
using UnityEngine;

[RequireComponent(typeof(UIScaleComponent))]
public class ShopItem : UIComponent
{
    [SerializeField] string TierID;

    [SerializeField] RectTransform RewardContainer;
    [SerializeField] RectTransform PurchaseContainer;
    [SerializeField] ButtonComponent Button;
    
    Tier Tier;

    GameObject PurchaseView;
    GameObject RewardView;

    IEnumerator currentScaleRoutine;

    UIScaleComponent scaleComponent;
    public UIScaleComponent ScaleComponent
    {
        get
        {
            if(scaleComponent == null)
                scaleComponent = GetComponent<UIScaleComponent>();
            return scaleComponent;
        }
    }
    
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
        RewardView = Tier.Reward.CreateView(RewardContainer).gameObject; 
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

    
    public virtual void Show(float delay, float duration, Action finished = null)
    {
        SetActive(true);
        
        if (!gameObject.activeInHierarchy)
        {
            finished?.Invoke();
            return;
        }
        
        if(currentScaleRoutine != null)
            StopCoroutine(currentScaleRoutine);

        StartCoroutine(currentScaleRoutine = ScaleRoutine(1, delay, duration, finished));
    }
    
    public virtual void Hide(float delay, float duration = 0.2f, Action finished = null)
    {
        if (!gameObject.activeInHierarchy)
        {
            finished?.Invoke();
            return;
        }
        
        if (currentScaleRoutine != null)
            StopCoroutine(currentScaleRoutine);

        StartCoroutine(currentScaleRoutine = ScaleRoutine(0, delay, duration, () =>
        {
            SetActive(false);
            
            finished?.Invoke();
        }));
    }
    
    IEnumerator ScaleRoutine(float targetScale, float delay, float duration, Action finished = null)
    {
        yield return new WaitForSeconds(delay);
        
        float sourceScale = ScaleComponent.Phase;
        float time = 0;
        
        while (time < duration)
        {
            ScaleComponent.Phase = Mathf.Lerp(sourceScale, targetScale, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        ScaleComponent.Phase = targetScale;
        
        finished?.Invoke();
    }

}
