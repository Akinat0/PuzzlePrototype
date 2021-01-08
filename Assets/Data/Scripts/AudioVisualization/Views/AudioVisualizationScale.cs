using UnityEngine;

namespace AudioVisualization.Views
{
    public class AudioVisualizationScale : AudioVisualizationBase
    {
        [SerializeField] float MinScale;
        [SerializeField] float MaxScale;
        
        protected override void UpdateView()
        {
            Transform.localScale = Vector3.one * Mathf.Clamp(SmoothAmplitude, MinScale, MaxScale);
        }
    }
}