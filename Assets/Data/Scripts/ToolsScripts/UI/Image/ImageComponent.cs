using System;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Tools.UI
{
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    public class ImageComponent : UIComponent, IColorReceiver
    {
        [SerializeField] Color color = Color.white; 
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

        public Color Color
        {
            get => color;
            set
            {
                if(color == value)
                    return;
                
                color = value;
                Image.color = value;
            }
        }

        public virtual void ApplyColor(params Color[] colors)
        {
            if(colors.Length > 0)
                Color = colors[0];
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            Image.color = color;
        }
    }
}