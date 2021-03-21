using Abu.Tools.UI;
using UnityEngine;

public class TextButtonComponent : ButtonComponent
{
    [SerializeField] string ButtonText = "Text";

    public string Text
    {
        get => TextField.Text;
        set => TextField.Text = value;
    }

    public TextComponent TextField
    {
        get
        {
            if (textField == null)
                textField = GetComponentInChildren<TextComponent>();
            
            return textField;
        }
    }

    private TextComponent textField;

    protected override void OnValidate()
    {
        base.OnValidate();
        Text = ButtonText;
    }
}
