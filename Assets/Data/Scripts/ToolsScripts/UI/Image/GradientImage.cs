using UnityEngine;
using UnityEngine.UI;

namespace Abu.Tools.UI
{
    public class GradientImage : Image, IColorReceiver
    {
        
        [SerializeField] Color firstColor = Color.white;
        [SerializeField] Color secondColor = Color.white;
        [SerializeField] bool inverseGradient;

        public Color FirstColor
        {
            get => firstColor;
            set
            {
                firstColor = value;
                SetVerticesDirty();
            }
        }
        
        public Color SecondColor
        {
            get => secondColor;
            set
            {
                secondColor = value;
                SetVerticesDirty();
            }
        }

        public bool InverseGradient
        {
            get => inverseGradient;
            set
            {
                if(inverseGradient == value)
                    return;
                
                inverseGradient = value;
                SetVerticesDirty();
            }
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);

            UIVertex vertex = new UIVertex();

            float yMin = float.MaxValue;
            float yMax = float.MinValue;
            
            for (int i = 0; i < toFill.currentVertCount; i++)
            {
                toFill.PopulateUIVertex(ref vertex, i);
                yMin = Mathf.Min(yMin, vertex.position.y);
                yMax = Mathf.Max(yMax, vertex.position.y);
            }
            
            for (int i = 0; i < toFill.currentVertCount; i++)
            {
                toFill.PopulateUIVertex(ref vertex, i);
                
                vertex.color = Color.LerpUnclamped(secondColor, firstColor,
                    inverseGradient 
                    ? 1 - vertex.position.y.Remap(yMin, yMax, 0, 1)
                    : vertex.position.y.Remap(yMin, yMax, 0, 1));
                
                toFill.SetUIVertex(vertex, i);
            }
        }

        public void ApplyColor(params Color[] colors)
        {
            if (colors.Length > 0)
                FirstColor = colors[0];
            if (colors.Length > 1)
                SecondColor = colors[1];
        }
    }
}