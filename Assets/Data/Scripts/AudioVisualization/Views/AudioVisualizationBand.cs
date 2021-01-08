using UnityEngine;

namespace AudioVisualization.Views
{
    public class AudioVisualizationBand : AudioVisualizationBase
    {
        [SerializeField] float MinScale = 0.1f;
        [SerializeField] float MaxScale = 2;
        [SerializeField] SpriteRenderer[] SpriteBand;
        
        protected override void UpdateView()
        {
            for (int i = 0; i < SmoothFrequencyBands.Length; i++)
            {
                Vector3 scale = SpriteBand[i].transform.localScale;
                scale.y = Mathf.Min(MinScale + SmoothFrequencyBands[i], MaxScale);
                SpriteBand[i].transform.localScale = scale;
            }
        }
    }
}