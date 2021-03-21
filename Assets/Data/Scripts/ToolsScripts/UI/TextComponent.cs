using TMPro;
using UnityEngine;

namespace Abu.Tools.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextComponent : UIComponent
    {
        [SerializeField] Color color = Color.white;
        [SerializeField] bool gradient = true;
        
        TextMeshProUGUI textMesh;
        public TextMeshProUGUI TextMesh
        {
            get
            {
                if (textMesh == null)
                    textMesh = GetComponent<TextMeshProUGUI>();

                return textMesh;
            }
        }
        
        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                    rectTransform = (RectTransform) transform;
                
                return rectTransform;
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
                UpdateColor();
            }
        }

        public string Text
        {
            get => TextMesh.text;
            set => TextMesh.text = value;
        }

        public float Alpha
        {
            get => TextMesh.alpha;
            set => TextMesh.alpha = value;
        }

        public float FontSize
        {
            get => TextMesh.fontSize;
            set => TextMesh.fontSize = value;
        }

        void UpdateColor()
        {
            if (gradient)
            {
                TextMesh.color = Color.white; 
                TextMesh.enableVertexGradient = true;
                VertexGradient vertexGradient = new VertexGradient(Color.white, Color.white, Color, Color);
                TextMesh.colorGradient = vertexGradient;
            }
            else
            {
                TextMesh.enableVertexGradient = false;
                TextMesh.color = Color;
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateColor();
        }

        void OnDidApplyAnimationProperties()
        {
            UpdateColor();
        }
        
    }
}