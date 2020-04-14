using Puzzle;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreenManager : ManagerView
{
    [SerializeField] private GameObject CompleteScreen;
    [SerializeField] private Button menuButton;

    [SerializeField] private FlatFXState startCompleteState;
    [SerializeField] private FlatFXState endCompleteState;

    [SerializeField] private AudioClip TaDa;

    private void Start()
    {
        CompleteScreen.SetActive(false);
    }

    public void ToMenu()
    {
        StopAllCoroutines();
        GameSceneManager.Instance.InvokeLevelClosed();
        CompleteScreen.SetActive(false);

        VFXManager.Instance.StopLevelCompleteSunshineEffect();
    }

    public void CreateReplyScreen()
    {
        CompleteScreen.SetActive(true);
        
        VFXManager.Instance.CallConfettiEffect();
        VFXManager.Instance.CallLevelCompleteSunshineEffect(GameSceneManager.Instance.GetPlayer().transform.position, startCompleteState, endCompleteState);

        AudioSource tadaSound = gameObject.AddComponent<AudioSource>();
        tadaSound.PlayOneShot(TaDa);
        
        
    }

    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(menuButton);
    }
    
}