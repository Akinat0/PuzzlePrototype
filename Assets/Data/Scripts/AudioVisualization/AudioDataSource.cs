using UnityEngine;

namespace AudioVisualization
{
    public class AudioDataSource : MonoBehaviour
    {
        public float[] Samples => spectrumData.Samples;
        public float[] FrequencyBands => spectrumData.FrequencyBands;
        public float[] SmoothFrequencyBands => spectrumData.SmoothFrequencyBands;
        public float Amplitude => spectrumData.Amplitude;
        public float SmoothAmplitude => spectrumData.SmoothAmplitude;

        AudioSource audioSource;
        AudioSpectrumData spectrumData;

        public bool IsPlaying { get; set; }

        void Update()
        {
            if(!IsPlaying)
                return;

            if (audioSource == null || spectrumData == null)
            {
                IsPlaying = false;
                return;
            }
            
            //It doesn't work properly during debug
            if(!audioSource.isPlaying)
                return;

            spectrumData.UpdateData();
        }

        public void AttachAudioSource(AudioSource source)
        {
            if (audioSource != null)
            {
                Debug.LogError("[AudioDataSource] AudioSource already exists on the component, detach previous first");
                return;
            }
            
            audioSource = source;
            spectrumData = new AudioSpectrumData(audioSource); 
            IsPlaying = true;
        }

        public void DetachAudioSource()
        {
            audioSource = null;
            spectrumData = null;
            IsPlaying = false;
        }
        
    }
}