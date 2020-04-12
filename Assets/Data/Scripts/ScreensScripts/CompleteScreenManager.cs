using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreenManager : ManagerView
{
    [SerializeField] private GameObject CompleteScreen;
    [SerializeField] private Button menuButton;
    [SerializeField] private Transform confettiHolder;

    private GameObject _confettiVfx;

    private void Start()
    {
        CompleteScreen.SetActive(false);
        _confettiVfx = Resources.Load<GameObject>("Prefabs/Confetti");
    }

    public void ToMenu()
    {
        GameSceneManager.Instance.InvokeLevelClosed();
        CompleteScreen.SetActive(false);
    }

    public void CreateReplyScreen()
    {
        CompleteScreen.SetActive(true);
        if (_confettiVfx != null)
            Instantiate(_confettiVfx, confettiHolder);
    }

    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetButtonColor(menuButton);
    }
}