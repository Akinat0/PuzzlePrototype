using System;
using Abu.Tools.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[Serializable]
public class LevelColorScheme
{
    [SerializeField] Color _ArrowColor = Color.white;
    [SerializeField] Color _ButtonColor = Color.white;
    [SerializeField] Color _AlternativeButtonColor = Color.white;
    [SerializeField] Color _TextColor = Color.black;
    [SerializeField] Color _TextColor2 = Color.black;
    [SerializeField] Color _TextColorLauncher = Color.black;

    public Color ArrowColor => _ArrowColor;
    public Color ButtonColor => _ButtonColor;
    public Color AlternativeButtonColor => _AlternativeButtonColor;
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
        button.AlternativeColor = AlternativeButtonColor;
    }
    
    public void SetButtonColor(TextButtonComponent button)
    {
        if (button == null)
        {
            Debug.LogError("ButtonComponent is null");
            return;
        }

        button.Color = ButtonColor;
        button.AlternativeColor = AlternativeButtonColor;
        button.TextField.color = TextColor;
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
