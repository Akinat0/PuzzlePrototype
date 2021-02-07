using UnityEngine;

namespace AudioVisualization
{
    public class AudioSpectrumData
    {
        public float[] Samples { get; private set; }
        public float[] FrequencyBands { get; private set; }
        public float[] SmoothFrequencyBands { get; private set; }
        public float Amplitude { get; private set; }
        public float SmoothAmplitude { get; private set; }

        public float FrequencyMultiplier { get; }

        AudioSource audioSource;
        FFTWindow fftWindow;

        readonly float[] frequencyDecrease;
        readonly float defaultDecrease;
        readonly float decreaseAcceleration;
        float hightestAmplitude = -1;
        
        
        public AudioSpectrumData(AudioSource source, float frequencyMultiplier = 10.0f, float defaultDecrease = 0.005f, float decreaseAcceleration = 1.085f, FFTWindow fftWindow = FFTWindow.Blackman)
        {
            audioSource = source;
            FrequencyMultiplier = frequencyMultiplier;
            this.defaultDecrease = defaultDecrease;
            this.decreaseAcceleration = decreaseAcceleration;
            this.fftWindow = fftWindow;
            Samples = new float[512];
            FrequencyBands = new float[8];
            SmoothFrequencyBands = new float[8];
            frequencyDecrease = new float[8];
        }

        public void UpdateData()
        {
            UpdateSpectrumData();
            CalculateFrequencyBands();
            CalculateAverageAmplitude();
            CalculateSmoothFrequencyBands();
        }

        void UpdateSpectrumData()
        {
            audioSource.GetSpectrumData(Samples, 0, fftWindow);
        }
        
        void CalculateFrequencyBands()
        {
            /*
             *
             * If you want deep understanding check there
             * https://www.youtube.com/watch?v=mHk3ZiKNH48&t=709s
             * 
             * 22050 / 512 = 43hertz per sample
             * 20 - 60 hertz
             * 60 - 250 hertz
             * 250 - 500 hertz
             * 500 - 2000 hertz
             * 2000 - 4000 hertz
             * 4000 - 6000 hertz
             * 6000 - 20000 hertz
             *
             * 0 - 2 = 86hertz 
             * 1 - 4 = 172 hertz - 87-258
             * 2 - 8 = 344 hertz - 259-602
             * 3 - 16 = 688 hertz - 603-1290
             * 4 - 32 = 1376 hertz - 1291-2666
             * 5 - 64 = 2752 hertz - 2667-5418
             * 6 -128 = 5504 hertz - 5419-10922
             * 7 -256 = 11008hertz - 10923-21930
             * 
             */
            
            int count = 0;
            float average = 0;
            
            for (int i = 0; i < 8; i++)
            {
                int samplesCount = (int) Mathf.Pow(2, i) * 2;

                if (i == 7) samplesCount += 2;

                for (int j = 0; j < samplesCount; j++)
                {
                    average += Samples[count] * (count + 1);
                    count++;
                }

                average /= count;

                FrequencyBands[i] = average * FrequencyMultiplier;
            }
        }

        void CalculateSmoothFrequencyBands()
        {
            for (int i = 0; i < 8; i++)
            {
                if (FrequencyBands[i] > SmoothFrequencyBands[i])
                {
                    SmoothFrequencyBands[i] = FrequencyBands[i];
                    frequencyDecrease[i] = defaultDecrease;
                }
                else
                {
                    SmoothFrequencyBands[i] -= frequencyDecrease[i];
                    frequencyDecrease[i] *= decreaseAcceleration;
                }
            }
        }
        void CalculateAverageAmplitude()
        {
            float amplitude = 0;
            float smoothAmplitude = 0;

            for (int i = 0; i < 8; i++)
            {
                amplitude += FrequencyBands[i];
                smoothAmplitude += SmoothFrequencyBands[i];
            }

            if (amplitude > hightestAmplitude)
                hightestAmplitude = amplitude;

            Amplitude = amplitude / hightestAmplitude;
            SmoothAmplitude = smoothAmplitude / hightestAmplitude;
        }
    }
}