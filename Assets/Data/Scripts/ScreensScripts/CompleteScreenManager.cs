using Abu.Tools.UI;
using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreenManager : ManagerView
{
    [SerializeField] private GameObject CompleteScreen;
    [SerializeField] private TextButtonComponent MenuButton;
    
    //TODO bad solution
    [SerializeField] private FlatFXState StartCompleteState;
    [SerializeField] private FlatFXState EndCompleteState;

    StarsManager StarsManager => GameSceneManager.Instance.LevelRootView.GetStarsManager(GameSceneManager.Instance.LevelConfig);
    bool StarsEnabled => GameSceneManager.Instance.LevelConfig.StarsEnabled;

    bool? IsNewRecord = null;
    
    private void Start()
    {
        CompleteScreen.SetActive(false);
        MenuButton.OnClick += OnMenuClick;
    }

    public void OnMenuClick()
    {
        StopAllCoroutines();
        VFXManager.Instance.StopLevelCompleteSunshineEffect();

        bool hideStars = IsNewRecord != null && !IsNewRecord.Value; 
        
        if (hideStars)
            StarsManager.HideStars();
        
        GameSceneManager.Instance.InvokeLevelClosed(hideStars);
        CompleteScreen.SetActive(false);
    }
    
    public void CreateReplyScreen(int stars, bool isNewRecord)
    {
        CompleteScreen.SetActive(true);
        
        IsNewRecord = isNewRecord;

        if (StarsEnabled)
            StarsManager.ShowStarsAnimation(stars, CallEffects);
        else
            CallEffects();
    }

    void CallEffects()
    {
        VFXManager.Instance.CallConfettiEffect();
        VFXManager.Instance.CallLevelCompleteSunshineEffect(GameSceneManager.Instance.Player.transform.position,
            StartCompleteState, EndCompleteState);
        VFXManager.Instance.CallWinningSound();
    }

    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(MenuButton);
    }

}