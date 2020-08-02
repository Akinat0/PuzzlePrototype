using System;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Tools.Skins
{
    [Serializable]
    public class SpriteColorSkin : SkinBase
    {
        [SerializeField] public Sprite Sprite;
        [SerializeField] public Color Color;
        
        public override void Apply(SpriteRenderer spriteRenderer)
        {
            spriteRenderer.color = Color;
            spriteRenderer.sprite = Sprite;
        }

        public override void Apply(Image image)
        {
            image.color = Color;
            image.sprite = Sprite;
        }
    }
}