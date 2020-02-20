using Puzzle;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    private bool _paused = false;
    public bool Paused => _paused;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void OnPause()
    {
        if(!Paused)
            PauseTheGame();
        else
        {
            ResumeTheGame();
        }
    }
    
    public void Replay()
    {
        pauseMenu.SetActive(false);
        GameSceneManager.Instance.InvokeResetLevel();
    }

    private void PauseTheGame()
    {
        _paused = true;
        pauseMenu.SetActive(true);
        GameSceneManager.Instance.InvokePauseLevel(true);
    }

    private void ResumeTheGame()
    {
        _paused = false;
        pauseMenu.SetActive(false);
        GameSceneManager.Instance.InvokePauseLevel(false);
    }

    public void VolumeSliderValueChanged(float value)
    {
        value = Mathf.Clamp01(value);
        SoundManager.Instance.SetVolume(value);
    }
}
