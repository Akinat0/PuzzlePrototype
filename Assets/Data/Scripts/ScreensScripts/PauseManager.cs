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

    private void PauseTheGame()
    {
        _paused = true;
        Time.timeScale = 0;  //Inline into event invoke
        pauseMenu.SetActive(true);
        GameSceneManager.Instance.InvokePauseLevel(true);
    }

    private void ResumeTheGame()
    {
        _paused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        GameSceneManager.Instance.InvokePauseLevel(false);
    }

    public void VolumeSliderValueChanged(float value)
    {
        value = Mathf.Clamp01(value);
        SoundManager.Instance.SetVolume(value);
    }
}
