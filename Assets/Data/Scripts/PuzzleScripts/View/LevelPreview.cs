using Puzzle;
using ScreensScripts;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LevelPreview : MonoBehaviour
{

    static int ShowID = Animator.StringToHash("Show");
    static int InstantID = Animator.StringToHash("Instant");
    Animator Animator;
    
    void Awake()
    {
        Animator = GetComponent<Animator>();
        Show(true);
    }

    void Show(bool instant = false)
    {
        Animator.SetBool(ShowID, true);
        Animator.SetBool(InstantID, instant);
    }

    void Hide()
    {
        Animator.SetBool(ShowID, false);
    }

    void OnEnable()
    {
        LauncherUI.GameEnvironmentLoadedEvent += GameEnvironmentLoadedEventHandler;
        LauncherUI.GameEnvironmentUnloadedEvent += GameEnvironmentUnloadedEventHandler;
    }

    void OnDisable()
    {
        LauncherUI.GameEnvironmentLoadedEvent -= GameEnvironmentLoadedEventHandler;
        LauncherUI.GameEnvironmentUnloadedEvent -= GameEnvironmentUnloadedEventHandler;
    }
    
    void GameEnvironmentLoadedEventHandler(GameSceneManager _)
    {
        Hide();
    }

    void GameEnvironmentUnloadedEventHandler(GameSceneUnloadedArgs _)
    {
        Show();
    }
}
