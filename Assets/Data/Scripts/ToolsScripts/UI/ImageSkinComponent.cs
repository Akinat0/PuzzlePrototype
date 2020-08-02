using Abu.Tools.Skins;
using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSkinComponent : UIComponent
{
    [SerializeField] protected SpriteColorSkin[] Skins;
    [SerializeField] protected int index;

    Image image;

    public Image Image
    {
        get
        {
            if (image == null)
                image = GetComponent<Image>();

            return image;
        }
    }

    public int Index
    {
        get => index;
        set
        {
            index = Mathf.Clamp(value, 0, Skins.Length-1);
            Skins[Index].Apply(Image);
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        Index = index;
    }
}
