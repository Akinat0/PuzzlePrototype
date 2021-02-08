using UnityEngine;

namespace AudioVisualization.Views
{
    public class AudioVisualizationMaskBand : AudioVisualizationBase
    {
        [SerializeField] float MinScale = 0.1f;
        [SerializeField] float MaxScale = 2;
        [SerializeField] SpriteMask[] SpriteMaskBand;
        
        protected override void UpdateView()
        {
            for (int i = 0; i < SmoothFrequencyBands.Length; i++)
            {
                Vector3 scale = SpriteMaskBand[i].transform.localScale;
                scale.y = Mathf.Min(MinScale + SmoothFrequencyBands[i], MaxScale);
                SpriteMaskBand[i].transform.localScale = scale;
            }
        }
    }
}