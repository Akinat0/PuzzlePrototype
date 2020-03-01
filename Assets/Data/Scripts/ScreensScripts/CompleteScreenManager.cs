using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreenManager : MonoBehaviour
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
    
    private void OnEnable()
    {
        GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
    }
    private void OnDisable()
    {
        GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
    }

    void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(menuButton);
    }
}
