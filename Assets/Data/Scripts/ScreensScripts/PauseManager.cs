using Abu.Tools.UI;
using Puzzle;
using TMPro;
using UnityEngine;

public class PauseManager : ManagerView
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] TextButtonComponent ContinueButton;
    [SerializeField] TextButtonComponent RestartButton;
    [SerializeField] ButtonComponent MenuButton;
    [SerializeField] ButtonComponent PauseButton;
    [SerializeField] TextComponent TimerField;
    
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
                Overlay.ChangePhase(0, 0.5f);
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
        Overlay.ChangePhase(0, 0.5f);
        GameSceneManager.Instance.InvokeResetLevel();
    }

    private void Pause()
    {
        paused = true;
        
        Overlay.ChangePhase(1, 0.4f);
        
        PauseMenu.SetActive(true);
        PauseButton.SetActive(false);
        GameSceneManager.Instance.InvokePauseLevel(true);
    }

    private void Resume()
    {
        paused = false;
        
        if(Overlay.Phase > Mathf.Epsilon)
            Overlay.ChangePhase(0, 0.4f);
        
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

    void LevelCompletedEvent_Handler(LevelCompletedEventArgs _)
    {
        PauseButton.gameObject.SetActive(false);
    }
}
