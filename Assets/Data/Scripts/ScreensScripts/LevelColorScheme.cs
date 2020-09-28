using System;
using Abu.Tools.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[Serializable]
public class LevelColorScheme
{
    [SerializeField] private Color _ArrowColor = Color.white;
    [SerializeField] private Color _ButtonColor = Color.white;
    [SerializeField] private Color _TextColor = Color.black;
    [SerializeField] private Color _TextColor2 = Color.black;
    [SerializeField] private Color _TextColorLauncher = Color.black;

    public Color ArrowColor => _ArrowColor;
    public Color ButtonColor => _ButtonColor;
    public Color TextColor => _TextColor;
    public Color TextColor2 => _TextColor2;
    public Color TextColorLauncher => _TextColorLauncher;

    public void SetButtonColor(ButtonComponent button)
    {
        if (button == null)
        {
            Debug.LogError("ButtonComponent is null");
            return;
        }

        button.Color = ButtonColor;
    }
    
    public void SetButtonColor(TextButtonComponent button)
    {
        if (button == null)
        {
            Debug.LogError("ButtonComponent is null");
            return;
        }

        button.Color = ButtonColor;
        button.TextField.color = TextColor;
    }
    public void SetButtonColor(Button button)
    {
        if (button == null)
        {
            Debug.LogError("Button is null");
            return;
        }
        
        bool defaultActiveState = button.gameObject.activeSelf;
        
        button.gameObject.SetActive(true);
        
        Image buttonImage = button.GetComponent<Image>();
        Text buttonText = button.GetComponentInChildren<Text>();

        if (buttonImage != null)
            buttonImage.color = ButtonColor;

        if (buttonText != null)
            buttonText.color = TextColor;
        
        button.gameObject.SetActive(defaultActiveState);
    }
    public void SetTextColor(Text text, bool alternateColor = false)
    {
        if (text == null)
        {
            Debug.LogError("Text is null");
            return;
        }

        text.color = alternateColor ? _TextColor2 : _TextColor;
    }
    public void SetTextColor(TextMeshProUGUI text, bool alternateColor = false)
    {
        if (text == null)
        {
            Debug.LogError("Text is null");
            return;
        }

        text.color = alternateColor ? _TextColor2 : _TextColor;
    }
    
}
