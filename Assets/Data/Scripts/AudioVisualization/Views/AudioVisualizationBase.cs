using UnityEngine;

namespace AudioVisualization.Views
{
    public abstract class AudioVisualizationBase : MonoBehaviour
    {
        [SerializeField] AudioDataSource audioDataSource;

        protected float[] FrequencyBands => audioDataSource.FrequencyBands;
        protected float[] SmoothFrequencyBands => audioDataSource.SmoothFrequencyBands;
        protected float Amplitude => audioDataSource.Amplitude;
        protected float SmoothAmplitude => audioDataSource.SmoothAmplitude;
        
        Transform cachedTransform;
        protected Transform Transform
        {
            get
            {
                if (cachedTransform == null)
                    cachedTransform = transform;

                return cachedTransform;
            }
        }
        
        protected void Update()
        {
            if(!audioDataSource.IsPlaying)
                return;
            
            UpdateView();
        }

        protected abstract void UpdateView();
    }
}