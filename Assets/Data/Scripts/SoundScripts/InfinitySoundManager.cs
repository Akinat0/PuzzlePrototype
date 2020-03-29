using Puzzle;
using UnityEngine;

public class InfinitySoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] themes;
    AudioSource _audioSource;

    private int _themeIndex = 0;
    private bool _play = false;

    private void Start()
    {
        //Fetch the AudioSource from the GameObject
        _audioSource = GetComponent<AudioSource>();

        //Set the original AudioClip as this clip
        _audioSource.clip = themes[0];

        //Output the current clip's length
        Debug.Log("Audio clip length : " + _audioSource.clip.length);
    }

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
        _audioSource.clip = clip;
        _audioSource.Play();

        Debug.Log("Play theme " + _audioSource.clip.name);
    }

    private void NextTheme()
    {
        _themeIndex++;
        PlayTheme(themes[_themeIndex]);
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
        PlayTheme(themes[_themeIndex]);
        _play = true;
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
