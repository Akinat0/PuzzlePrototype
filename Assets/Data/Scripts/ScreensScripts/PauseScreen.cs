using System;
using Abu.Tools.UI;
using Puzzle;
using UnityEngine;

public class PauseScreen : ScreenComponent
{
    [SerializeField] TextButtonComponent ContinueButton;
    [SerializeField] TextButtonComponent RestartButton;
    [SerializeField] ButtonComponent MenuButton;

    public override bool Show(Action finished = null)
    {
        if (!base.Show(finished))
            return false;

        ContinueButton.ShowComponent();
        RestartButton.ShowComponent();
        MenuButton.ShowComponent();

        return true;
    }
    
    public override bool Hide(Action finished = null)
    {
        if (!base.Hide(finished))
            return false;

        ContinueButton.HideComponent();
        RestartButton.HideComponent();
        MenuButton.HideComponent();

        return true;
    }
    
    void Start()
    {
        ContinueButton.HideComponent(0);
        RestartButton.HideComponent(0);
        MenuButton.HideComponent(0);

        ContinueButton.OnClick += OnContinueClick;
        RestartButton.OnClick += OnReplayClick;
        MenuButton.OnClick += OnMenuClick;
    }

    void OnContinueClick()
    {
        Resume();
    }
    
    void Resume()
    {
        GameSceneManager.Instance.InvokePauseLevel(false, false);
        Hide();
    }
    
    void OnMenuClick()
    {
        GameSceneManager.Instance.InvokeLevelClosed();
        Hide();
    }
    
    void OnReplayClick()
    {
        GameSceneManager.Instance.InvokeResetLevel();
        Hide();
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
    }

    void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(ContinueButton);
        levelColorScheme.SetButtonColor(RestartButton);
        levelColorScheme.SetButtonColor(MenuButton);
    }

    void PauseLevelEvent_Handler(bool pause)
    {
        if (pause && GameSceneManager.Instance.CurrentHearts > 0)
            Show();
    }
}
