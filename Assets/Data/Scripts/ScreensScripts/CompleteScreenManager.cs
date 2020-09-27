using System;
using Abu.Tools.UI;
using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreenManager : ManagerView
{
    [SerializeField] private GameObject CompleteScreen;
    [SerializeField] private TextButtonComponent MenuButton;
    
    StarsManager StarsManager => GameSceneManager.Instance.LevelRootView.GetStarsManager(GameSceneManager.Instance.LevelConfig);
    bool StarsEnabled => GameSceneManager.Instance.LevelConfig.StarsEnabled;

    bool? IsNewRecord = null;

    Action CallEffectsAction;
    
    private void Start()
    {
        CompleteScreen.SetActive(false);
        MenuButton.OnClick += OnMenuClick;
    }

    public void OnMenuClick()
    {
        StopAllCoroutines();
        VFXManager.Instance.StopLevelCompleteSunshineEffect();

        bool hideStars = IsNewRecord != null && !IsNewRecord.Value && StarsEnabled; 
        
        if (hideStars)
            StarsManager.HideStars();
        
        GameSceneManager.Instance.InvokeLevelClosed(hideStars);
        CompleteScreen.SetActive(false);
    }
    
    public void CreateReplyScreen(int stars, bool isNewRecord)
    {
        CompleteScreen.SetActive(true);
        
        IsNewRecord = isNewRecord;

        CallEffectsAction = CallEffects;
        
        if (StarsEnabled)
            StarsManager.ShowStarsAnimation(stars);
        
        CallEffects();
    }

    void CallEffects()
    {
        VFXManager.Instance.CallWinningSound();
    }

    void InvokeCallEffects()
    {
        Action action = CallEffectsAction;
        CallEffectsAction = null;
        
        action?.Invoke();
    }
    
    
    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(MenuButton);
    }

}