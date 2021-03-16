using UnityEditor;
using UnityEngine;

namespace Abu.Tools.UI
{
    public class GradientImageComponent : ImageComponent
    {
        static readonly int GradientColorSource = Shader.PropertyToID("_GradientColorSource");
        static readonly int GradientColorTarget = Shader.PropertyToID("_GradientColorTarget");
        
        [SerializeField] Color firstColor;
        [SerializeField] Color secondColor;
        
        public Color FirstColor
        {
            get => firstColor;
            set
            {
                if(firstColor == value)
                    return;
                
                firstColor = value;
                UpdateMaterial();
            }
        }
        
        public Color SecondColor
        {
            get => secondColor;
            set
            {
                if(secondColor == value)
                    return;
                
                secondColor = value;
                UpdateMaterial();
            }
        }

        public override void ApplyColor(params Color[] colors)
        {
            if (colors.Length > 0)
                FirstColor = colors[0];
            
            if(colors.Length > 1)
                SecondColor = colors[1];
        }

        void UpdateMaterial()
        {
#if UNITY_EDITOR
            if(Image.material.shader.name != "Shader Graphs/GradientUnlit")
                Image.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Data/Materials/Common/GradientUnlit.mat");
#endif
            
            Image.materialForRendering.SetColor(GradientColorSource, FirstColor);
            Image.materialForRendering.SetColor(GradientColorTarget, SecondColor);
        }

        void Awake()
        {
            Image.material = Instantiate(Image.material);
            UpdateMaterial();
        }
        void Reset()
        {
            UpdateMaterial();
        }

        void OnDidApplyAnimationProperties()
        {
            UpdateMaterial();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateMaterial();
        }
    }
}