using Puzzle;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreenManager : ManagerView
{
    [SerializeField] private GameObject CompleteScreen;
    [SerializeField] private Button menuButton;
    [SerializeField] private Transform[] confettiHolders;

    [SerializeField] private FlatFXState startCompleteState;
    [SerializeField] private FlatFXState endCompleteState;

    [SerializeField] private AudioClip TaDa;
    private GameObject _confettiVfx;
    private Coroutine _completeEffectRoutine;

    private void Start()
    {
        CompleteScreen.SetActive(false);
        _confettiVfx = Resources.Load<GameObject>("Prefabs/Confetti");
    }

    public void ToMenu()
    {
        StopAllCoroutines();
        GameSceneManager.Instance.InvokeLevelClosed();
        CompleteScreen.SetActive(false);

        VFXManager.StopLevelCompleteEffect();
    }

    public void CreateReplyScreen()
    {
        CompleteScreen.SetActive(true);

        foreach (Transform confettiHolder in confettiHolders)
        {
            if (_confettiVfx != null)
                Instantiate(_confettiVfx, confettiHolder);
        }
        
        VFXManager.CallLevelCompleteEffect(GameSceneManager.Instance.GetPlayer().transform.position, startCompleteState, endCompleteState);

        AudioSource tadaSound = gameObject.AddComponent<AudioSource>();
        tadaSound.PlayOneShot(TaDa);
        
        
    }

    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(menuButton);
    }
    
}