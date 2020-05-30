using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : ManagerView
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Text timerField;
    
    private bool _paused = false;
    public bool Paused => _paused;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void OnPause()
    {
        if (!Paused)
            PauseTheGame();
        else if (timerField != null)
        {
            pauseMenu.SetActive(false);
            StartCoroutine(CountdownRoutine(timerField, () =>
            {
                timerField.gameObject.SetActive(false);
                _paused = false;
                GameSceneManager.Instance.InvokePauseLevel(false);
            }));
        }
        else
            ResumeTheGame();
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

    


    protected override void OnEnable()
    {
        GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
    }

    protected override void OnDisable()
    {
        GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
        GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
    }

    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(continueButton);
        levelColorScheme.SetButtonColor(restartButton);
        levelColorScheme.SetButtonColor(menuButton);
        levelColorScheme.SetButtonColor(pauseButton);
        if(timerField != null)
            levelColorScheme.SetTextColor(timerField, true);
    }

    void LevelCompletedEvent_Handler()
    {
        pauseButton.gameObject.SetActive(false);
    }
}
