using Abu.Tools;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundView : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;

    void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = 
            Vector3.one * ScreenScaler.FitHorizontal(m_SpriteRenderer);
        
    }
/*
    void CreateClippingZone()
    {
        m_SpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        
        m_ClippingZone = gameObject.AddComponent<SpriteMask>();
        Texture2D texture = Resources.Load<Texture2D>("Textures/white_pattern");

        if (texture == null)
        {
            Debug.LogError("Textures/white_pattern is missing");
            return;
        }
        
        Vector2 camSize = ScreenScaler.CameraSize();
        
        Sprite sprite = Sprite.Create(texture, 
            new Rect(0, 0, camSize.x/transform.localScale.x, camSize.y/transform.localScale.y),
            new Vector2(0.5f, 0.5f), 1);
        
        m_ClippingZone.sprite = sprite;
    }*/
}
