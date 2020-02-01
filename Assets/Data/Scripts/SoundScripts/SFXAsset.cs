using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu()]
public class SFXAsset : ScriptableObject
{
    public AudioClip[] SoundClips;

    [Range(-100, 0)]
    public float VolumeDB = 0f;
    [Range(0, 20)]
    public float RandomVolumeDB = 0f;

    [Range(-12, 12)]
    public float Pitch = 0f;
    [Range(0, 12)]
    public float RandomPitch = 0f;
    [HideInInspector]
    public float pitchCoef = Mathf.Pow(2f, 1.0f / 12f);

    public bool Loop = false;

    [Range(0f, 1f)]
    public float SpacialBlend = 1f;

    protected AudioClip RandomClip()
    {
        int clipIndex;
        if (SoundClips.Length > 1)
        {
            clipIndex = Random.Range(0, SoundClips.Length);
        }
        else
        {
            clipIndex = 0;
        }
        return SoundClips[clipIndex];
    }

    public void Play(AudioSource voice)
    {
        AudioClip _clip = RandomClip();
        float _volDB = VolumeDB + Random.Range(RandomVolumeDB, 0);
        float _vol = Mathf.Clamp01(Mathf.Pow(10.0f, _volDB / 20.0f));

        float _pitchRandom = Random.Range(Pitch - RandomPitch, Pitch + RandomPitch);
        float _pitch = Mathf.Clamp(Mathf.Pow(pitchCoef, _pitchRandom), 0f, 2f);

        //Debug.Log("SFXAsset clip is " + _clip);
        ConfigureSource(voice, _vol, _pitch, SpacialBlend);
        voice.PlayOneShot(_clip);
    }

    public void ConfigureSource(AudioSource voice, AudioClip clip, float vol, float pitch, float spacialBlend)
    {
        voice.clip = clip;
        voice.volume = vol;
        voice.pitch = pitch;
        voice.spatialBlend = spacialBlend;
    }
    public void ConfigureSource(AudioSource voice, float vol, float pitch, float spacialBlend)
    {
        voice.volume = vol;
        voice.pitch = pitch;
        voice.spatialBlend = spacialBlend;
    }
}