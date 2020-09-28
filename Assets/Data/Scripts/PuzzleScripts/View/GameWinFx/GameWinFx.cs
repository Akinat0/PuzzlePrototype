using Puzzle;
using ScreensScripts;
using UnityEngine;

public abstract class GameWinFx : MonoBehaviour
{
    protected abstract void Show();
    protected abstract void Hide();

    void Subscribe()
    {
        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
    }

    void Unsubscribe()
    {
        GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
    }
    
    void OnEnable()
    {
        LauncherUI.GameSceneLoadedEvent += GameSceneLoadedEvent_Handler;
    }

    void OnDisable()
    {
        LauncherUI.GameSceneLoadedEvent -= GameSceneLoadedEvent_Handler;
    }

    void GameSceneLoadedEvent_Handler(GameSceneManager _)
    {
        Subscribe();
    }

    void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
    {
        Show();
    }

    void LevelClosedEvent_Handler()
    {
        Unsubscribe();
        Hide();
    }
}
