using System;
using Puzzle;
using UnityEngine;

public class CompleteScreen : ScreenComponent
{
    [SerializeField] TextButtonComponent MenuButton;
    
    StarsController StarsController => GameSceneManager.Instance.LevelRootView.GetStarsManager(GameSceneManager.Instance.LevelConfig);
    bool StarsEnabled => GameSceneManager.Instance.LevelConfig.StarsEnabled;

    int cachedStarsAmount;
    
    void Start()
    {
        MenuButton.HideComponent();
        
        MenuButton.OnClick += OnMenuClick;
        cachedStarsAmount = GameSceneManager.Instance.LevelConfig.StarsAmount;
    }

    public override bool Show(Action finished = null)
    {
        if (!base.Show(finished))
            return false;

        MenuButton.ShowComponent();

        if (StarsEnabled)
            StarsController.ShowStarsAnimation(GameSceneManager.Instance.LevelConfig.StarsAmount);
        
        CallEffects();

        return true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
    }

    public void OnMenuClick()
    {
        StopAllCoroutines();

        int starsAmount = GameSceneManager.Instance.LevelConfig.StarsAmount;
        bool hideStars = StarsEnabled && starsAmount < cachedStarsAmount; 
        
        if (hideStars)
            StarsController.HideStars();
        
        GameSceneManager.Instance.InvokeLevelClosed(hideStars);
    }

    void CallEffects()
    {
        VFXManager.Instance.CallWinningSound();
    }

    void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(MenuButton);
    }

}