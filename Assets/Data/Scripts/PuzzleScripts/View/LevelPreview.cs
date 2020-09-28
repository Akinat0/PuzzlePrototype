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
        LauncherUI.GameSceneLoadedEvent += GameSceneLoadedEvent_Handler;
        LauncherUI.GameSceneUnloadedEvent += GameSceneUnloadedEvent_Handler;
    }

    void OnDisable()
    {
        LauncherUI.GameSceneLoadedEvent -= GameSceneLoadedEvent_Handler;
        LauncherUI.GameSceneUnloadedEvent -= GameSceneUnloadedEvent_Handler;
    }
    
    void GameSceneLoadedEvent_Handler(GameSceneManager _)
    {
        Hide();
    }

    void GameSceneUnloadedEvent_Handler(GameSceneUnloadedArgs _)
    {
        Show();
    }
}
