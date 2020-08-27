using System;
using Abu.Tools;
using Abu.Tools.SceneTransition;
using PuzzleScripts;
using ScreensScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public static event Action<int> PlayerLosedHpEvent;
        public static event Action<int> EnemyDiedEvent;
        public static event Action<EnemyParams> CreateEnemyEvent;
        public static event Action<LevelColorScheme> SetupLevelEvent;
        public static event Action LevelClosedEvent;
        public static event Action<LevelCompletedEventArgs> LevelCompletedEvent;
        public static event Action<LevelPlayAudioEventArgs> PlayAudioEvent;
        public static event Action<EnemyBase> EnemyAppearedOnScreenEvent;
        public static event Action<CutsceneEventArgs> CutsceneStartedEvent;
        public static event Action<CutsceneEventArgs> CutsceneEndedEvent;

        [SerializeField] private RuntimeAnimatorController cameraAnimatorController;
        [SerializeField] private CompleteScreenManager completeScreenManager;
        [SerializeField] private ReplayScreenManager replayScreenManager;
        [SerializeField] private AudioClip theme;
        [SerializeField] private Transform gameSceneRoot;

        public Transform GameSceneRoot => gameSceneRoot;
        public LevelConfig LevelConfig => _levelConfig;
        public LevelRootView LevelRootView => _levelRootView;

        public Player Player => player;

        LevelRootView _levelRootView;
        private Player player;
        private Animator _gameCameraAnimator;
        private static readonly int Shake = Animator.StringToHash("shake");
        private LevelConfig _levelConfig;
        
        protected BubbleDialog _currentDialog;

        protected virtual void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Debug.LogError("There's more than one GameSceneManager in the scene");
        }

        protected virtual void Start() { }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void ShakeCamera()
        {
            _gameCameraAnimator.SetTrigger(Shake);
        }

        public void SetupScene(GameObject _player, LevelConfig config, LevelRootView levelRootView)
        {
            player = _player.AddComponent<Player>();
            player.sides = config.PuzzleSides.ToArray();
            
            _player.AddComponent<PlayerInput>();
            FindObjectOfType<SpawnerBase>().PlayerEntity = _player;
            
            _gameCameraAnimator = LauncherUI.Instance.MainCamera.gameObject.AddComponent<Animator>();
            _gameCameraAnimator.runtimeAnimatorController = cameraAnimatorController;

            _levelConfig = config;
            _levelRootView = levelRootView; 

            if (config.ColorScheme != null)
                InvokeSetupLevel(config.ColorScheme);

            foreach (Booster booster in Account.GetActiveBoosters())
                booster.Use();

            InvokeResetLevel();
        }

        void UnloadScene()
        {
            Destroy(player.GetComponent<PlayerInput>());
            Destroy(player.GetComponent<Player>());
            Destroy(_gameCameraAnimator);
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }

        void CallEndgameMenu()
        {
            replayScreenManager.CreateReplyScreen();
        }

        void CallCompleteMenu(int stars, bool isNewRecord)
        {
            completeScreenManager.CreateReplyScreen(stars, isNewRecord);
        }
        
        //TODO move it to player view
        public void ShowDialog(string message, float time = -1)
        {
            BubbleDialog newDialog = BubbleDialog.Create(
                bubbleDialog =>
                {
                    bubbleDialog.transform.parent = GameSceneRoot;
                    bubbleDialog.transform.localScale =
                        ScreenScaler.FitHorizontalPart(bubbleDialog.Background, 0.35f) *
                        Vector2.one;
                
                    //Put dialog on the top right puzzle angle
                    float halfOfPuzzleWidth = ScreenScaler.PartOfScreen(Player.PlayerView.PartOfScreen / 2).x;
                    bubbleDialog.transform.position = Vector2.one * halfOfPuzzleWidth;
                });
        
            if (_currentDialog != null)
                _currentDialog.Hide(() => newDialog.Show(message));
            else
                newDialog.Show(message);

            _currentDialog = newDialog;
            _currentDialog.SetRenderLayer(RenderLayer.VFX, 102);

            if (time > 0)
                //If time is specified we will close window in this time
                _currentDialog.Invoke(() => _currentDialog.Hide(), time);
        
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

        public void InvokePlayerRevive()
        {
            Debug.Log("PlayerRevive Invoked");
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
            UnloadScene();
            LauncherUI.Instance.InvokeGameSceneUnloaded(
                new GameSceneUnloadedArgs(GameSceneUnloadedArgs.GameSceneUnloadedReason.LevelClosed, showStars));
        }
        
        public void InvokeLevelCompleted()
        {
            Debug.Log("LevelComplete Invoked");
            LevelCompletedEvent?.Invoke(new LevelCompletedEventArgs(_levelConfig));
            
            //Get stars amount from hp
            int stars = Player.Hp.Remap(0, Player.DEFAULT_HP, 0, 3);

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
    }
}