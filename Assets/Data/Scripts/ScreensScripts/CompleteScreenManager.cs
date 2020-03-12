using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreenManager : ManagerView
{
    [SerializeField] private GameObject CompleteScreen;
    [SerializeField] private Button menuButton;
    
    private void Start() 
    {
        CompleteScreen.SetActive(false);
    }

    public void ToMenu() 
    {
        GameSceneManager.Instance.InvokeLevelClosed();
        CompleteScreen.SetActive(false);
    }

    public void CreateReplyScreen() 
    {
        CompleteScreen.SetActive(true);
    }
    
    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(menuButton);
    }
}
