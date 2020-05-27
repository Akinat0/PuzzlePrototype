using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreenManager : ManagerView
{
    [SerializeField] private GameObject CompleteScreen;
    [SerializeField] private Button menuButton;

    [SerializeField] private FlatFXState startCompleteState;
    [SerializeField] private FlatFXState endCompleteState;

    [SerializeField] private StarsView starsView;
    

    private void Start()
    {
        CompleteScreen.SetActive(false);
    }

    public void ToMenu()
    {
        StopAllCoroutines();
        VFXManager.Instance.StopLevelCompleteSunshineEffect();

        if (starsView == null)
        {
            GameSceneManager.Instance.InvokeLevelClosed();
            CompleteScreen.SetActive(false);
        }
        else
        {
            starsView.HideStars(() => {
                GameSceneManager.Instance.InvokeLevelClosed();
                CompleteScreen.SetActive(false); 
            });
        }
    }
    
    public void CreateReplyScreen(int remainHp, int totalHp)
    {
        CompleteScreen.SetActive(true);
        
        int stars = remainHp.Remap(0, totalHp, 0, 3); //Get stars amount from hp

        if (starsView != null)
        {
            starsView.ShowStars(stars, CallEffects);
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
            startCompleteState, endCompleteState);
        VFXManager.Instance.CallWinningSound();
    }
    
    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(menuButton);
    }

}