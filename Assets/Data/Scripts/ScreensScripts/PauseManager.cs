using Abu.Tools.UI;
using Puzzle;
using TMPro;
using UnityEngine;

public class PauseManager : ManagerView
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private TextButtonComponent ContinueButton;
    [SerializeField] private TextButtonComponent RestartButton;
    [SerializeField] private ButtonComponent MenuButton;
    [SerializeField] private ButtonComponent PauseButton;
    [SerializeField] private TextMeshProUGUI TimerField;
    
    private bool paused = false;
    public bool Paused => paused;

    private void Start()
    {
        PauseMenu.SetActive(false);
        
        ContinueButton.OnClick += OnPauseClick;
        RestartButton.OnClick += OnReplayClick;
        MenuButton.OnClick += OnMenuClick;
        PauseButton.OnClick += OnPauseClick;
    }

    private void OnPauseClick()
    {
        if (!Paused)
            Pause();
        else
        {
            if (TimerField != null)
            {
                PauseMenu.SetActive(false);
                StartCoroutine(CountdownRoutine(TimerField, () =>
                {
                    TimerField.gameObject.SetActive(false);
                    Resume();
                }));
            }
            else
            {
                Resume();
            }
        }
    }
    
//    private void VolumeSliderValueChanged(float value)
//    {
//        value = Mathf.Clamp01(value);
//        SoundManager.Instance.SetVolume(value);
//    }

    private void OnMenuClick()
    {
        GameSceneManager.Instance.InvokeLevelClosed();
        PauseMenu.SetActive(false);
    }
    
    private void OnReplayClick()
    {
        paused = false;
        PauseMenu.SetActive(false);
        PauseButton.SetActive(true);
        GameSceneManager.Instance.InvokeResetLevel();
    }

    private void Pause()
    {
        paused = true;
        PauseMenu.SetActive(true);
        PauseButton.SetActive(false);
        GameSceneManager.Instance.InvokePauseLevel(true);
    }

    private void Resume()
    {
        paused = false;
        PauseMenu.SetActive(false);
        PauseButton.SetActive(true);
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
        levelColorScheme.SetButtonColor(ContinueButton);
        levelColorScheme.SetButtonColor(RestartButton);
        levelColorScheme.SetButtonColor(MenuButton);
        levelColorScheme.SetButtonColor(PauseButton);
        if(TimerField != null)
            levelColorScheme.SetTextColor(TimerField, true);
    }

    void LevelCompletedEvent_Handler()
    {
        PauseButton.gameObject.SetActive(false);
    }
}
