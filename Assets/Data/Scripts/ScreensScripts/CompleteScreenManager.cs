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

    [SerializeField] private StarsView StarsView;
    

    private void Start()
    {
        CompleteScreen.SetActive(false);
        MenuButton.OnClick += OnMenuClick;
    }

    public void OnMenuClick()
    {
        StopAllCoroutines();
        VFXManager.Instance.StopLevelCompleteSunshineEffect();

        if (StarsView == null)
        {
            GameSceneManager.Instance.InvokeLevelClosed();
            CompleteScreen.SetActive(false);
        }
        else
        {
            StarsView.HideStars(() => {
                GameSceneManager.Instance.InvokeLevelClosed();
                CompleteScreen.SetActive(false); 
            });
        }
    }
    
    public void CreateReplyScreen(int remainHp, int totalHp)
    {
        CompleteScreen.SetActive(true);
        
        int stars = remainHp.Remap(0, totalHp, 0, 3); //Get stars amount from hp

        if (StarsView != null)
        {
            StarsView.ShowStarsAnimation(stars, CallEffects);
        }
        else
        {
            CallEffects();
        }
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