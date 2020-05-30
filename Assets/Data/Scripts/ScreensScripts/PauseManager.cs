using System.Collections;
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
        if(!Paused)
            PauseTheGame();
        else
        {
            StartCoroutine(ResumeTheGame());
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
    

    private IEnumerator ResumeTheGame()
    {
        timerField.gameObject.SetActive(true);
        pauseMenu.SetActive(false);
        for (int i = 3; i > 0; i--)
        {
            timerField.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }
        timerField.gameObject.SetActive(false);
        _paused = false;
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
    }

    void LevelCompletedEvent_Handler()
    {
        pauseButton.gameObject.SetActive(false);
    }
}
