using Puzzle;
using UnityEngine;

public class InfinitySoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] themes;
    AudioSource _audioSource;

    private int _themeIndex = 0;
    private bool _play = false;
    
    void Update()
    {
        if (!_play)
            return;

        if (!_audioSource.isPlaying)
        {
            NextTheme();
        }
    }

    private void PlayTheme(AudioClip clip)
    {
        if(_audioSource != null) 
            Destroy(_audioSource.gameObject);

        _audioSource = new GameObject("Theme " + clip.name).AddComponent<AudioSource>();
        _audioSource.transform.SetParent(transform);
        _audioSource.clip = clip;
        _audioSource.Play();

        Debug.Log("Play theme " + _audioSource.clip.name);
    }

    private void NextTheme()
    {
        PlayTheme(themes[_themeIndex]);
        _themeIndex++;
    }
    
    protected void OnEnable()
    {
        GameSceneManager.GameStartedEvent += GameStartedEvent_Handler;
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }

    protected void OnDisable()
    {
        GameSceneManager.GameStartedEvent -= GameStartedEvent_Handler;
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
    }

    private void GameStartedEvent_Handler()
    {
        NextTheme();
    }

    private void PauseLevelEvent_Handler(bool paused)
    {
        if (_audioSource == null)
            return;
        _play = !paused;
        if(_play)
            _audioSource.Play();
        else
            _audioSource.Pause();
    }

    private void ResetLevelEvent_Handler()
    {
        if (_audioSource == null)
            return;
        _audioSource.Stop();
        _audioSource.time = 0;
    }
}
