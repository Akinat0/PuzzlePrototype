using System;
using System.Collections.Generic;
using Abu.Tools.UI;
using UnityEngine;

public class ErrorWindow : Window
{
    public enum ErrorType
    {
        InternetConnection,
        AdSkipped
    }

    static readonly Dictionary<ErrorType, string> Messages = new Dictionary<ErrorType, string>
    {
        {ErrorType.InternetConnection, "Internet disabled. Please enable internet and try again"},
        {ErrorType.AdSkipped, "Don't skip advertisement to receive the reward"}
    };

    static readonly Dictionary<ErrorType, string> Titles = new Dictionary<ErrorType, string>
    {
        {ErrorType.InternetConnection, "Connection issue"},
        {ErrorType.AdSkipped, "Ad skipped"}
        
    };
    
    public static ErrorWindow Create(Action onSuccess, ErrorType errorType)
    {
        Debug.LogWarning($"[ErrorWindow] ShowError window. Error type {errorType}");
        
        Window prefab = Resources.Load<Window>("UI/Window");
        ErrorWindow window = ConvertTo<ErrorWindow>(Instantiate(prefab, Root));

        void CreateContent(RectTransform rectTransform)
        { 
            TextComponent.Create(rectTransform, Messages[errorType]);
        } 

        void OnSuccess()
        {
            onSuccess?.Invoke();
            window.Hide();
        }
        
        window.Instantiate(CreateContent, OnSuccess, Titles[errorType], "Ok");
        return window;
    }
}
