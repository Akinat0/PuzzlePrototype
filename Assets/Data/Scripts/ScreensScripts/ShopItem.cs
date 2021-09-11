using System;
using System.Collections;
using Abu.Tools.UI;
using UnityEngine;

[RequireComponent(typeof(UIScaleComponent))]
public class ShopItem : UIComponent, IListElement 
{
    public static ShopItem Create(Tier tier)
    {
        ShopItem shopItem = Instantiate(Resources.Load<ShopItem>("UI/ShopItem"));
        shopItem.Tier = tier;
        shopItem.ScaleComponent.Phase = 0;
        return shopItem;
    }

    [SerializeField] RectTransform RewardContainer;
    [SerializeField] RectTransform PurchaseContainer;
    [SerializeField] ButtonComponent Button;

    public Tier Tier { get; private set; }

    public ButtonComponent BuyButton => Button; 

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

    void OnClick()
    {
        Tier.Obtain(() => RewardWindow.Create(Tier.Reward));
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

    public void LinkToList(Transform container)
    {
        transform.parent = container;
        
        CreateView();
        
        Button.OnClick += OnClick;
        Tier.OnAvailableChangedEvent += OnAvailableChangedEvent_Handler;
        Tier.OnTierValueChangedEvent += OnTierValueChangedEvent_Handler;
    }

    public Vector2 Size => RectTransform.rect.size;
    public Vector3 Position => RectTransform.position;
}
