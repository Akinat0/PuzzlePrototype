using UnityEngine;

namespace AudioVisualization.Views
{
    public class AudioVisualizationColorBand : AudioVisualizationBase
    {
        [SerializeField] Gradient Gradient;
        [SerializeField] float MinValue = 0;
        [SerializeField] float MaxValue = 5;
        [SerializeField] SpriteRenderer[] SpriteBand;

        protected override void UpdateView()
        {
            for (int i = 0; i < SmoothFrequencyBands.Length; i++)
            {
                float gradientPhase = SmoothFrequencyBands[i].Remap(MinValue, MaxValue, 0, 1);
                Color color = Gradient.Evaluate(gradientPhase);
                SpriteBand[i].color = color;
            }
        }
    }
}