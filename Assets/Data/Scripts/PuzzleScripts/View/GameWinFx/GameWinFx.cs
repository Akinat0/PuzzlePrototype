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
        LauncherUI.GameEnvironmentLoadedEvent += GameEnvironmentLoadedEventHandler;
    }

    void OnDisable()
    {
        LauncherUI.GameEnvironmentLoadedEvent -= GameEnvironmentLoadedEventHandler;
    }

    void GameEnvironmentLoadedEventHandler(GameSceneManager _)
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
