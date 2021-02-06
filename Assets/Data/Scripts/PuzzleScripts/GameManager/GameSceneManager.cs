using System;
using System.Collections.Generic;
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

        public static event Action GameStartedEvent;
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
        public static event Action<CutsceneEventArgs> CutsceneStartedEvent;
        public static event Action<CutsceneEventArgs> CutsceneEndedEvent;
        public static event Action<Booster> ApplyBoosterEvent;
        public static event Action<int> HeartsAmountChangedEvent;
        public static event Action<string> TimelineEvent;

        [SerializeField] RuntimeAnimatorController cameraAnimatorController;
        [SerializeField] CompleteScreenManager completeScreenManager;
        [SerializeField] ReplayScreenManager replayScreenManager;
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
        }

        void OnDestroy()
        {
            Instance = null;
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

        void CallEndgameMenu()
        {
            replayScreenManager.CreateReplyScreen(Session.ReviveUsed);
        }

        void CallCompleteMenu(int stars, bool isNewRecord)
        {
            completeScreenManager.CreateReplyScreen(stars, isNewRecord);
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

            Session = new GameSession(levelConfig);
            
            CurrentHearts = DEFAULT_HEARTS;
            TotalHearts = DEFAULT_HEARTS;

            ResetLevelEvent?.Invoke();
            GameStartedEvent?.Invoke();
            InvokePauseLevel(false); //Unpausing
        }

        public void InvokePauseLevel(bool pause)
        {
            if (pause)
                TimeManager.Pause();
            else
                TimeManager.Unpause();
            
            Debug.Log("PauseLevel Invoked " + (pause ? "paused" : "unpaused"));
            PauseLevelEvent?.Invoke(pause);
        }

        public void InvokePlayerDied()
        {
            InvokePauseLevel(true);
            Debug.Log("PlayerDied Invoked");
            PlayerDiedEvent?.Invoke();
            CallEndgameMenu();
        }

        public void InvokePlayerLosedHp()
        {
            Debug.Log("PlayerLosedHp Invoked");
            PlayerLosedHpEvent?.Invoke();
            
            CurrentHearts--;
            
            if(CurrentHearts == 0)
                InvokePlayerDied();
        }

        public void InvokeEnemyDied(EnemyBase enemy)
        {
            Debug.Log("EnemyDied Invoked");
            EnemyDiedEvent?.Invoke(enemy);
        }

        public void InvokeGameStarted()
        {
            Debug.Log("GameStarted Invoked");
            GameStartedEvent?.Invoke();
            InvokePauseLevel(false); //Unpausing
        }

        public void InvokePlayerRevive()
        {
            Debug.Log("PlayerRevive Invoked");
            CurrentHearts = TotalHearts;
            PlayerReviveEvent?.Invoke();
            InvokePauseLevel(false);
        }
        
        public void InvokeCreateEnemy(EnemyParams @params)
        {
            Debug.Log("CreateEnemy Invoked");
            CreateEnemyEvent?.Invoke(@params);
        }
        
        public void InvokeLevelClosed(bool showStars = true)
        {
            InvokePauseLevel(true);
            Debug.Log("LevelClosed Invoked");
            LevelClosedEvent?.Invoke();
            DestroyEnvironment();
            LauncherUI.Instance.InvokeGameEnvironmentUnloaded(
                new GameSceneUnloadedArgs(GameSceneUnloadedArgs.GameSceneUnloadedReason.LevelClosed, showStars, levelConfig));
        }
        
        public void InvokeLevelCompleted()
        {
            Debug.Log("LevelComplete Invoked");
            LevelCompletedEvent?.Invoke(new LevelCompletedEventArgs(levelConfig, currentHearts,
                Session.SessionBoosters.ToArray(), Session.ReviveUsed));
            
            //Get stars amount from difficulty
            int stars = (int)LevelConfig.DifficultyLevel;
            
            bool isNewRecord = LevelConfig.StarsAmount < stars; 
            
            if(isNewRecord)
                LevelConfig.StarsAmount = stars;
            
            CallCompleteMenu(stars, isNewRecord);
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
        
        public void InvokeCutsceneStarted(CutsceneEventArgs args)
        {
            Debug.Log("CutsceneStarted Invoked " + args.SceneID);
            InvokePauseLevel(true);
            CutsceneStartedEvent?.Invoke(args);
        }
        
        public void InvokeCutsceneEnded(CutsceneEventArgs args)
        {
            Debug.Log("CutsceneEnded Invoked " + args.SceneID);
            InvokePauseLevel(false);
            CutsceneEndedEvent?.Invoke(args);
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
    }
}