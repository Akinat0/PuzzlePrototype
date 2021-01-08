using System;
using UnityEngine;

namespace AudioVisualization.Views
{
    public class AudioVisualizationColor : AudioVisualizationBase
    {
        
        [SerializeField] Gradient Gradient;
        [SerializeField] float MinValue = 0;
        [SerializeField] float MaxValue = 5;
        [SerializeField] SpriteRenderer Target;
        protected override void UpdateView()
        {
            float gradientPhase = SmoothAmplitude.Remap(MinValue, MaxValue, 0, 1);
            Color color = Gradient.Evaluate(gradientPhase);
            Target.color = color;
        }
    }
}