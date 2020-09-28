using Abu.Tools.UI;
using TMPro;
using UnityEngine;

public class TextButtonComponent : ButtonComponent
{
    [SerializeField] private string ButtonText = "Text";

    public string Text
    {
        get => TextField.text;
        set => TextField.text = value;
    }

    public TextMeshProUGUI TextField
    {
        get
        {
            if (textField == null)
                textField = GetComponentInChildren<TextMeshProUGUI>();
            
            return textField;
        }
    }

    private TextMeshProUGUI textField;

    protected override void OnValidate()
    {
        base.OnValidate();
        Text = ButtonText;
    }
}
