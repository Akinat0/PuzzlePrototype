using System;
using ScreensScripts;
using UnityEngine;

namespace Puzzle{
public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    public static event Action GameStartedEvent;
    public static event Action ResetLevelEvent;
    public static event Action<bool> PauseLevelEvent;
    public static event Action PlayerDiedEvent;
    public static event Action<int> PlayerLosedHpEvent;
    public static event Action<int> EnemyDiedEvent;

    
    [SerializeField] private Animator gameCameraAnimator;

    [SerializeField] private Player player;
    [SerializeField] private ReplayScreenManager replayScreenManager;

    private static readonly int Shake = Animator.StringToHash("shake");
    
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShakeCamera()
    { 
        gameCameraAnimator.SetTrigger(Shake);
    }

    public Vector3 GetPlayerPos()
    {
        return player.transform.position;
    }
    void CallEndgameMenu()
    {
        replayScreenManager.CreateReplyScreen();
    }

    //////////////////
    //Event Invokers//
    //////////////////
    
    public void InvokeResetLevel()
    {
        ResetLevelEvent?.Invoke();
    }

    public void InvokePauseLevel(bool pause)
    {
        PauseLevelEvent?.Invoke(pause);
    }

    public void InvokePlayerDied()
    {
        InvokePauseLevel(true);
        PlayerDiedEvent?.Invoke();
        CallEndgameMenu();
        InvokeResetLevel();
    }

    public void InvokePlayerLosedHp(int hp)
    {
        PlayerLosedHpEvent?.Invoke(hp);
    }

    public void InvokeEnemyDied(int score)
    {
        EnemyDiedEvent?.Invoke(score);
    }
    
    public void InvokeGameStarted()
    {
        GameStartedEvent?.Invoke();
        InvokePauseLevel(false); //Unpausing
    }
}
}