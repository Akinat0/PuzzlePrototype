using System;
using System.Collections;
using Abu.Tools.UI;
using ScreensScripts;
using UnityEngine;

[RequireComponent(typeof(UIScaleComponent), typeof(CanvasGroup))]
public class Window : UIComponent
{
    [SerializeField] protected TextButtonComponent OkButton;
    [SerializeField] protected RectTransform ContentContainer;
    [SerializeField] protected TextComponent Title;
    
    protected static RectTransform Root => LauncherUI.Instance.UiManager.Root;

    protected OverlayView Overlay;

    CanvasGroup canvasGroup;
    protected CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            
            return canvasGroup;
        }
    }

    UIScaleComponent scaleComponent;
    public UIScaleComponent ScaleComponent
    {
        get
        {
            if (scaleComponent == null)
                scaleComponent = GetComponent<UIScaleComponent>();
            return scaleComponent;
        }
    }

    IEnumerator currentScaleRoutine;

    public static Window Create(Action<RectTransform> createContent, Action onSuccess, string title, string okText)
    {
        Window prefab = Resources.Load<Window>("UI/Window");
        Window window = Instantiate(prefab, Root);
        window.Instantiate(createContent, onSuccess, title, okText);
        return window;
    }

    protected static T ConvertTo<T>(Window window) where T : Window
    {
        T newWindow = window.gameObject.AddComponent<T>();

        newWindow.Title = window.Title;
        newWindow.OkButton = window.OkButton;
        newWindow.ContentContainer = window.ContentContainer;
        
        Destroy(window);

        return newWindow;
    }
    
    protected virtual void Instantiate(Action<RectTransform> createContent, Action onSuccess, string title, string okText)
    {
        RectTransform.SetAsLastSibling();

        Title.Text = title;
        
        Overlay = OverlayView.Create<BlurOverlayView>(Root, RectTransform.GetSiblingIndex() - 1);
        Overlay.OnClick += onSuccess;

        OkButton.Text = okText;
        OkButton.OnClick += onSuccess;
        
        createContent?.Invoke(ContentContainer);

        Show();
    }

    public void Hide()
    {
        Overlay.ChangePhase(0, 0.2f);
        
        if(currentScaleRoutine != null)
            StopCoroutine(currentScaleRoutine);

        StartCoroutine(currentScaleRoutine = ScaleRoutine(0, 0.2f, () =>
        {
            Destroy(Overlay.gameObject);
            Destroy(gameObject);
        }));
    }

    public void Show()
    {
        ScaleComponent.Phase = 0;
        CanvasGroup.alpha = 0;
        Overlay.Phase = 0;
        
        Overlay.ChangePhase(1, 0.2f);

        if(currentScaleRoutine != null)
            StopCoroutine(currentScaleRoutine);

        StartCoroutine(currentScaleRoutine = ScaleRoutine(1, 0.2f));
    }

    IEnumerator ScaleRoutine(float targetValue, float duration, Action finished = null)
    {
        float sourceScale = ScaleComponent.Phase;
        float sourceAlpha = CanvasGroup.alpha;

        float time = 0;
            
        while (time < duration)
        {
            float phase = time / duration;
            
            ScaleComponent.Phase = Mathf.Lerp(sourceScale, targetValue, phase);
            CanvasGroup.alpha = Mathf.Lerp(sourceAlpha, targetValue, phase);
            
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        ScaleComponent.Phase = targetValue;
        CanvasGroup.alpha = targetValue;
        
        finished?.Invoke();
    }
    
}
