using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LevelColorScheme
{
    [SerializeField] private Color _ArrowColor = Color.white;
    [SerializeField] private Color _ButtonColor = Color.white;
    [SerializeField] private Color _TextColor = Color.black;
    [SerializeField] private Color _TextColor2 = Color.black;

    public Color ArrowColor => _ArrowColor;
    public Color ButtonColor => _ButtonColor;
    public Color TextColor => _TextColor;
    public Color TextColor2 => _TextColor2;

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
    public void SetTextColor(Text text)
    {
        if (text == null)
        {
            Debug.LogError("Text is null");
            return;
        }

        text.color = _TextColor2;
    }
}
