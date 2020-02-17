using System;
using PuzzleScripts;
using ScreensScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public static event Action<EnemyParams> CreateEnemyEvent;
        public static event Action LevelClosedEvent;

        [SerializeField] private RuntimeAnimatorController cameraAnimatorController;
        [SerializeField] private ReplayScreenManager replayScreenManager;
        [SerializeField] private AudioClip theme;
        [SerializeField] private Transform gameSceneRoot;

        public Transform GameSceneRoot => gameSceneRoot;

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
        FindObjectOfType<SpawnerBase>().PlayerEntity = _player;
        if (Camera.main != null)
        {
            _gameCameraAnimator = Camera.main.gameObject.AddComponent<Animator>();
            _gameCameraAnimator.runtimeAnimatorController = cameraAnimatorController;
        }
        else
        {
            Debug.LogError("Camera is null");
        }

        SoundManager.Instance.LevelThemeClip = theme;
        
        // TODO  actions with background and gameRoot
        ResetLevelEvent?.Invoke();
        InvokeGameStarted();
    }

    void UnloadScene()
    {
        Destroy(_player.GetComponent<PlayerInput>());
        Destroy(_player.GetComponent<Player>());
        Destroy(_gameCameraAnimator);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    public Player GetPlayer()
    {
        return _player;
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
        Debug.Log("ResetLevel Invoked");
        ResetLevelEvent?.Invoke();
    }

    public void InvokePauseLevel(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        Debug.Log("PauseLevel Invoked " + (pause ? "paused" : "unpaused"));
        PauseLevelEvent?.Invoke(pause);
    }

    public void InvokePlayerDied()
    {
        InvokePauseLevel(true);
        Debug.Log("PlayerDied Invoked");
        PlayerDiedEvent?.Invoke();
        CallEndgameMenu(); 
        InvokeResetLevel();
    }

    public void InvokePlayerLosedHp(int hp)
    {
        Debug.Log("PlayerLosedHp Invoked, hp was " + hp);
        PlayerLosedHpEvent?.Invoke(hp);
    }

    public void InvokeEnemyDied(int score)
    {
        Debug.Log("EnemyDied Invoked");
        EnemyDiedEvent?.Invoke(score);
    }
    
    public void InvokeGameStarted()
    {
        Debug.Log("GameStarted Invoked");
        GameStartedEvent?.Invoke();
        InvokePauseLevel(false); //Unpausing
    }

    public void InvokeCreateEnemy(EnemyParams @params)
    {
        Debug.Log("CreateEnemy Invoked");
        CreateEnemyEvent?.Invoke(@params);
    }
    
    public void InvokeLevelClosed()
    {
        InvokePauseLevel(true);
        Debug.Log("Event Closed Invoked");
        LevelClosedEvent?.Invoke();
        UnloadScene();
        LauncherUI.Instance.InvokeGameSceneUnloaded();
    }
}
}
