using System;
using UnityEngine;
using Abu.Tools;
using Abu.Tools.UI;
using Puzzle;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace ScreensScripts
{
    public class LauncherUI : MonoBehaviour
    {
        public static LauncherUI Instance;
        
        public static event Action<PlayLauncherEventArgs> PlayLauncherEvent;
        public static event Action<GameSceneManager> GameEnvironmentLoadedEvent;
        public static event Action<GameSceneUnloadedArgs> GameEnvironmentUnloadedEvent;
        public static event Action<LevelChangedEventArgs> LevelChangedEvent;
        public static event Action<ShowCollectionEventArgs> ShowCollectionEvent;
        public static event Action<CloseCollectionEventArgs> CloseCollectionEvent;
        public static event Action<Achievement> AchievementReceived; 


        [SerializeField] AsyncLoader asyncLoader;
        [SerializeField] MainMenuUIManager uiManager;
        [SerializeField] MainCameraComponent mainCamera;

        public MainMenuUIManager UiManager => uiManager;
        public MainCameraComponent MainCamera => mainCamera; 

        public LevelConfig LevelConfig => _levelConfig;
        
        private GameSceneManager GameSceneManager;

        private LevelConfig _levelConfig;

        LevelRootView actualLevelRootView;
        Transform playerEntity;
        
        public TextGroupComponent LauncherTextGroup { get; private set; }
        
        private void Awake()
        {
            Instance = this;
            SceneManager.LoadScene("PuzzleAtlasScene", LoadSceneMode.Additive);
            LauncherTextGroup = TextGroupComponent.AttachTo(gameObject);
        }

        void PlayLevel(LevelConfig config)
        {
            if (asyncLoader.gameObject != null && config != null && !string.IsNullOrEmpty(config.SceneID))
                asyncLoader.LoadGameEnvironment(config.SceneID, InvokeGameEnvironmentLoaded);
        }

        public void InvokePlayLauncher(PlayLauncherEventArgs args)
        {
            Debug.Log("PlayLauncher Invoked");
            _levelConfig = args.LevelConfig;
            actualLevelRootView = args.LevelRootView;
            PlayLevel(args.LevelConfig);
            PlayLauncherEvent?.Invoke(args);
        }
        
        public void InvokeGameEnvironmentLoaded(GameSceneManager gameSceneManager)
        {
            Debug.Log("GameEnvironmentLoaded Invoked");
            gameSceneManager.SetupScene(playerEntity.gameObject, _levelConfig, actualLevelRootView); //LauncherUI is launcher scene root
            GameEnvironmentLoadedEvent?.Invoke(gameSceneManager);
        }

        public void InvokeGameEnvironmentUnloaded(GameSceneUnloadedArgs args)
        {
            Debug.Log("GameEnvironmentUnloaded Invoked");
            GameEnvironmentUnloadedEvent?.Invoke(args);

            //Unpause game anyway
            TimeManager.DefaultTimeScale = 1;
            TimeManager.Unpause();
        }

        public void InvokeLevelChanged(LevelChangedEventArgs args)
        {
            Debug.Log("LevelChanged Invoked");
            playerEntity = args.PlayerView.transform;
            _levelConfig = args.LevelConfig;
            LevelChangedEvent?.Invoke(args);
        }

        public void InvokeShowCollection(ShowCollectionEventArgs args)
        {
            Debug.Log("LevelChanged Invoked");
            ShowCollectionEvent?.Invoke(args);
        }

        public void InvokeCloseCollection(CloseCollectionEventArgs args)
        {
            Debug.Log("CloseCollection Invoked");
            
            if(args.PlayerView != null)
                playerEntity = args.PlayerView.transform;
            
            CloseCollectionEvent?.Invoke(args);
        }
        
        public void InvokeAchievementReceived(Achievement achievement)
        {
            Debug.Log($"Achievement {achievement.Name} received Invoked");
            AchievementReceived?.Invoke(achievement);
        }

    }
}