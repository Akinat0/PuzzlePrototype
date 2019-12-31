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

    [SerializeField] private RuntimeAnimatorController cameraAnimatorController;
    [SerializeField] private ReplayScreenManager replayScreenManager;
    
    
    private Player _player;
    private Animator _gameCameraAnimator;
    private static readonly int Shake = Animator.StringToHash("shake");
    
    void Awake()
    {
        Instance = this;
    }

    public void ShakeCamera()
    { 
        _gameCameraAnimator.SetTrigger(Shake);
    }

    public void SetupScene(GameObject _player, GameObject background, GameObject gameRoot)
    {
        this._player = _player.AddComponent<Player>();
        _player.AddComponent<PlayerInput>();
        FindObjectOfType<Spawner>().playerEntity = _player;
        if (Camera.main != null)
        {
            _gameCameraAnimator = Camera.main.gameObject.AddComponent<Animator>();
            _gameCameraAnimator.runtimeAnimatorController = cameraAnimatorController;
        }
        else
        {
            Debug.LogError("Camera is null");
        }

        // TO-DO  actions with background and gameRoot
        ResetLevelEvent?.Invoke();
        InvokeGameStarted();
    }

    public Vector3 GetPlayerPos()
    {
        return _player.transform.position;
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