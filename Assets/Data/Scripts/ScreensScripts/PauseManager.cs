using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button pauseButton;
    
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
    
    public void VolumeSliderValueChanged(float value)
    {
        value = Mathf.Clamp01(value);
        SoundManager.Instance.SetVolume(value);
    }

    public void ToMenu()
    {
        GameSceneManager.Instance.InvokeLevelClosed();
        pauseMenu.SetActive(false);
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


    private void OnEnable()
    {
        GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
    }
    private void OnDisable()
    {
        GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
        GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
    }

    void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(continueButton);
        levelColorScheme.SetButtonColor(restartButton);
        levelColorScheme.SetButtonColor(menuButton);
        levelColorScheme.SetButtonColor(pauseButton);
    }

    void LevelCompletedEvent_Handler()
    {
        pauseButton.gameObject.SetActive(false);
    }
}
