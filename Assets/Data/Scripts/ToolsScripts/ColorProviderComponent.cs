using UnityEngine;

public class ColorProviderComponent : MonoBehaviour
{
    
    public Color Color = Color.white;
    public SpriteRenderer[] sprites;

    private Color prevColor = Color.white;
    
    private void Update()
    {
        if (Color != prevColor)
        {
            foreach (SpriteRenderer sprite in sprites)
            {
                if (sprite != null)
                    sprite.color = Color;
            }

            prevColor = Color;
        }
    }
}
