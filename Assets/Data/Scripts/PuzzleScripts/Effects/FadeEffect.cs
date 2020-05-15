using Abu.Tools;
using UnityEngine;

public class FadeEffect
{
    public const float DefaultAlpha = 0.2f;
    
    public SpriteRenderer Sprite { get; private set; }

    public void setActive(bool active)
    {
        Sprite.gameObject.SetActive(active);
    }

    public void Unload()
    {
        Object.Destroy(Sprite.gameObject);   
    }

    public FadeEffect(Transform parent, string sortingLayer, int sortingOrder)
    {
        GameObject fadeObject = new GameObject("Fade");
        Sprite = fadeObject.AddComponent<SpriteRenderer>();
        Sprite.transform.parent = parent;
        Sprite.sprite = Resources.Load<Sprite>("Textures/white_pattern");
        Sprite.sortingLayerName = sortingLayer;
        Sprite.sortingOrder = sortingOrder;
        Sprite.color = new Color(0, 0, 0, 0.2f);
        Sprite.transform.localScale = ScreenScaler.ScaleToFillScreen(Sprite);
    }
    
    
}
