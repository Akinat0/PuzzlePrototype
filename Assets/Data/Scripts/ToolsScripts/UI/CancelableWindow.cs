using System;
using Abu.Tools.UI;
using UnityEngine;

public class CancelableWindow : Window
{
    [SerializeField] protected TextButtonComponent CancelButton;
    
    public static CancelableWindow Create(Action<RectTransform> createContent, Action onSuccess, Action onCancel, string title, string okText, string cancelText)
    {
        CancelableWindow prefab = Resources.Load<CancelableWindow>("UI/CancelableWindow");
        CancelableWindow window = Instantiate(prefab, Root);
        window.Initialize(createContent, onSuccess, onCancel, title, okText, cancelText);
        return window;
    }
    
    protected static T ConvertTo<T>(CancelableWindow window) where T : CancelableWindow
    {
        T newWindow = window.gameObject.AddComponent<T>();

        newWindow.Title = window.Title;
        newWindow.OkButton = window.OkButton;
        newWindow.ContentContainer = window.ContentContainer;
        newWindow.CancelButton = window.CancelButton;
        
        Destroy(window);
        
        return newWindow;
    }
    
    protected void Initialize(Action<RectTransform> createContent, Action onSuccess, Action onCancel, string title, string okText, string cancelText)
    {
        Transform.SetAsLastSibling();

        Title.Text = title;
        
        Overlay = OverlayView.Create<BlurOverlayView>(Root, Transform.GetSiblingIndex() - 1);
        Overlay.OnClick += onCancel;

        CancelButton.Text = cancelText;
        CancelButton.OnClick += onCancel;
        
        OkButton.Text = okText;
        OkButton.OnClick += onSuccess;
        
        createContent?.Invoke(ContentContainer);
        
        Transform.localScale = Vector2.zero;
        Show();
    }
}
