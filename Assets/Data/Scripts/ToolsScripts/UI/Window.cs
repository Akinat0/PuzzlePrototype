using System;
using Abu.Tools.UI;
using DG.Tweening;
using ScreensScripts;
using TMPro;
using UnityEngine;

public class Window : UIComponent
{
    [SerializeField] protected TextButtonComponent OkButton;
    [SerializeField] protected RectTransform ContentContainer;
    [SerializeField] protected TextMeshProUGUI Title;
    
    protected static RectTransform Root => LauncherUI.Instance.UiManager.Root;

    protected OverlayView Overlay;
    protected RectTransform Transform => (RectTransform) transform;
    

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
        Transform.SetAsLastSibling();

        Title.text = title;
        
        Overlay = OverlayView.Create<BlurOverlayView>(Root, Transform.GetSiblingIndex() - 1);
        Overlay.OnClick += onSuccess;

        OkButton.Text = okText;
        OkButton.OnClick += onSuccess;
        
        createContent?.Invoke(ContentContainer);
        
        Transform.localScale = Vector2.zero;
        Show();
    }

    public void Hide()
    {
        
        Transform.DOScale(Vector2.zero, 0.2f);
        Overlay.ChangePhase(0, 0.2f, () =>
        {
            Destroy(Overlay.gameObject);
            Destroy(gameObject);
        });
    }

    public void Show()
    {
        Transform.DOScale(Vector2.one, 0.2f);
        Overlay.ChangePhase(1, 0.2f);
    }
    
}
