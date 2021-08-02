using System;
using System.Linq;
using Abu.Tools;
using PuzzleScripts;
using ScreensScripts;
using UnityEngine;

namespace Puzzle
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance;

        public static event Action LevelStartedEvent;
        public static event Action ResetLevelEvent;
        public static event Action<bool> PauseLevelEvent;
        public static event Action PlayerReviveEvent;
        public static event Action PlayerDiedEvent;
        public static event Action PlayerLosedHpEvent;
        public static event Action<EnemyBase> EnemyDiedEvent;
        public static event Action<EnemyParams> CreateEnemyEvent;
        public static event Action<LevelColorScheme> SetupLevelEvent;
        public static event Action LevelClosedEvent;
        public static event Action<LevelCompletedEventArgs> LevelCompletedEvent;
        public static event Action<LevelPlayAudioEventArgs> PlayAudioEvent;
        public static event Action<EnemyBase> EnemyAppearedOnScreenEvent;
        public static event Action<Booster> ApplyBoosterEvent;
        public static event Action<int> HeartsAmountChangedEvent;
        public static event Action<string> TimelineEvent;
        public static event Action PerfectKillEvent;
        
        
        [SerializeField] Transform gameSceneRoot;

        public Transform GameSceneRoot => gameSceneRoot;
        public LevelConfig LevelConfig => levelConfig;
        public LevelRootView LevelRootView => levelRootView;

        Transform playerCachedParent;

        public Player Player => player;

        public int CurrentHearts
        {
            get => currentHearts;
            set
            {
                if (currentHearts == value)
                    return;

                currentHearts = value;

                if (currentHearts > totalHearts)
                    totalHearts = currentHearts;
                
                InvokeHeartsAmountChanged(currentHearts);
            }
        }

        public int TotalHearts
        {
            get => totalHearts;
            set => totalHearts = value;
        }

        public GameSession Session { get; private set; }
        public Requests Requests { get; private set; }

        public const int DEFAULT_HEARTS = 5;
        
        int currentHearts = DEFAULT_HEARTS;
        int totalHearts = DEFAULT_HEARTS;

        LevelRootView levelRootView;
        Player player;
        LevelConfig levelConfig;

        protected virtual void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Debug.LogError("There's more than one GameSceneManager in the scene");

            Requests = new Requests();
        }

        void OnDestroy()
        {
            Instance = null;
        }

        void OnApplicationFocus(bool hasFocus)
        {
            #if !UNITY_EDITOR
            if (!hasFocus && Session != null && !Session.Result.HasValue) InvokePauseLevel(true);
            #endif
        }

        void OnApplicationPause(bool pause)
        {
            #if !UNITY_EDITOR
            if (pause && Session != null && !Session.Result.HasValue) InvokePauseLevel(true);
            #endif
        }

        public void SetupScene(GameObject _player, LevelConfig config, LevelRootView levelRootView)
        {
            player = _player.AddComponent<Player>();
            player.sides = config.PuzzleSides.ToArray();
            
            _player.AddComponent<PlayerInput>();
            
            FindObjectOfType<SpawnerBase>().PlayerEntity = _player;

            playerCachedParent = player.transform.parent;
            player.transform.parent = GameSceneRoot;

            levelConfig = config;
            this.levelRootView = levelRootView; 

            InvokeLevelStarted();
            
            if (config.ColorScheme != null)
                InvokeSetupLevel(config.ColorScheme);

            InvokeResetLevel();

            foreach (Booster booster in Account.GetActiveBoosters())
                booster.Use();
        }

        void DestroyEnvironment()
        {
            player.transform.parent = playerCachedParent;
            Destroy(player.GetComponent<PlayerInput>());
            Destroy(player.GetComponent<Player>());
            Destroy(gameObject);
        }
        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        //////////////////
        //Event Invokers//
        //////////////////

        public void InvokeSetupLevel(LevelColorScheme colorScheme)
        {
            Debug.Log("SetupLevel Invoked");
            SetupLevelEvent?.Invoke(colorScheme);
        }

        public void InvokeResetLevel()
        {
            Debug.Log("ResetLevel Invoked");

            //It could be a moment after play button pressed and there is no session yet
            if (Session != null)
            {
                foreach (Booster booster in Session.ActiveBoosters)
                    booster.Release();
            }
            
            Session = new GameSession(levelConfig);
            
            CurrentHearts = DEFAULT_HEARTS;
            TotalHearts = DEFAULT_HEARTS;

            ResetLevelEvent?.Invoke();
            
            InvokePauseLevel(false);
        }

        public void InvokePauseLevel(bool pause, bool instant = true)
        {
            if (pause)
                TimeManager.Pause();
            else
                TimeManager.Unpause(instant);
            
            Debug.Log("PauseLevel Invoked " + (pause ? "paused" : "unpaused"));
            PauseLevelEvent?.Invoke(pause);
        }

        public void InvokePlayerDied()
        {
            InvokePauseLevel(true);
            Debug.Log("PlayerDied Invoked");
            
            //TODO uncomment this to enable auto restart
            // this.InvokeRealtime(InvokeResetLevel, 2f);
            // return;
            
            PlayerDiedEvent?.Invoke();
        }

        public void InvokePlayerLosedHp()
        {
            CurrentHearts--; 
            
            Debug.Log("PlayerLosedHp Invoked");
            PlayerLosedHpEvent?.Invoke();

            if(CurrentHearts == 0)
                InvokePlayerDied();
        }

        public void InvokeEnemyDied(EnemyBase enemy)
        {
            Debug.Log("EnemyDied Invoked");
            EnemyDiedEvent?.Invoke(enemy);
        }

        public void InvokeLevelStarted()
        {
            Debug.Log("LevelStarted Invoked");
            LevelStartedEvent?.Invoke();
            InvokePauseLevel(false); //Unpausing
        }

        public void InvokePlayerRevive()
        {
            Debug.Log("PlayerRevive Invoked");
            CurrentHearts = TotalHearts;
            PlayerReviveEvent?.Invoke();
            InvokePauseLevel(false, false);
        }
        
        public void InvokeCreateEnemy(EnemyParams @params)
        {
            Debug.Log("CreateEnemy Invoked");
            CreateEnemyEvent?.Invoke(@params);
        }
        
        public void InvokeLevelClosed(bool showStars = true)
        {
            foreach (Booster booster in Session.ActiveBoosters)
                booster.Release();
            
            InvokePauseLevel(true);
            
            Debug.Log("LevelClosed Invoked");
            LevelClosedEvent?.Invoke();
            
            Requests.Dispose();
            DestroyEnvironment();
            
            LauncherUI.Instance.InvokeGameEnvironmentUnloaded(
                new GameSceneUnloadedArgs(GameSceneUnloadedArgs.GameSceneUnloadedReason.LevelClosed, showStars, levelConfig));
        }
        
        public void InvokeLevelCompleted()
        {
            Debug.Log("LevelComplete Invoked");

            if (LevelConfig.StarsEnabled)
            {
                LevelConfig.ObtainSecondStar();
                LevelConfig.TryObtainThirdStar(CurrentHearts);
            }
            
            LevelCompletedEvent?.Invoke(new LevelCompletedEventArgs(levelConfig, currentHearts,
                Session.ActiveBoosters.ToArray(), Session.ReviveUsed));
        }
        
        public void InvokePlayAudio(LevelPlayAudioEventArgs args)
        {
            Debug.Log("PlayAudio Invoked " + args.AudioClip.name);
            PlayAudioEvent?.Invoke(args);
        }
        
        public void InvokeEnemyAppearedOnScreen(EnemyBase enemyBase)
        {
            Debug.Log("EnemyAppearedOnScreen Invoked " + enemyBase.Type);
            EnemyAppearedOnScreenEvent?.Invoke(enemyBase);
        }
        
        public void InvokeApplyBooster(Booster booster)
        {
            Debug.Log("Apply Booster Invoked " + booster.Name);
            ApplyBoosterEvent?.Invoke(booster);
        }
        
        void InvokeHeartsAmountChanged(int hearts)
        {
            Debug.Log("HeartsAmountChanged Invoked, new hearts amount " + hearts);
            HeartsAmountChangedEvent?.Invoke(hearts);
        }

        public void InvokeTimelineEvent(string EventData)
        {
            Debug.Log("Timeline event invoked, data: " + EventData);
            TimelineEvent?.Invoke(EventData);
        }

        public void InvokePerfectKill()
        {
            Debug.Log("Perfect kill event invoked");
            Session.IncrementCombo();
            PerfectKillEvent?.Invoke();
        }
    }
}