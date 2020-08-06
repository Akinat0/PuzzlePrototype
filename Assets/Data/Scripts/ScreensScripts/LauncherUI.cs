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
        public static event Action<GameSceneManager> GameSceneLoadedEvent;
        public static event Action GameSceneUnloadedEvent;
        public static event Action<LevelChangedEventArgs> LevelChangedEvent;
        public static event Action<ShowCollectionEventArgs> ShowCollectionEvent;
        public static event Action<CloseCollectionEventArgs> CloseCollectionEvent;


        [SerializeField] private AsyncLoader asyncLoader;
        [SerializeField] private Transform playerEntity;
        [SerializeField] private GameObject backgroundContainer;

        [SerializeField] MainMenuUIManager uiManager;
        [SerializeField] MainCameraComponent mainCamera;

        public MainMenuUIManager UiManager => uiManager;
        public MainCameraComponent MainCamera => mainCamera; 

        public LevelConfig LevelConfig => _levelConfig;
        
        private GameSceneManager GameSceneManager;

        private LevelConfig _levelConfig;

        private void Awake()
        {
            Instance = this;
            SceneManager.LoadScene("PuzzleAtlasScene", LoadSceneMode.Additive);
        }

        void PlayLevel(LevelConfig config)
        {
            if (asyncLoader.gameObject != null && config != null && !string.IsNullOrEmpty(config.SceneID))
                asyncLoader.LoadScene(config.SceneID, InvokeGameSceneLoaded);
        }

        public void InvokePlayLauncher(PlayLauncherEventArgs args)
        {
            Debug.Log("PlayLauncher Invoked");
            _levelConfig = args.LevelConfig;
            PlayLevel(args.LevelConfig);
            PlayLauncherEvent?.Invoke(args);
        }
        
        public void InvokeGameSceneLoaded(GameSceneManager gameSceneManager)
        {
            Debug.Log("GameSceneLoaded Invoked");
            gameSceneManager.SetupScene(playerEntity.gameObject, _levelConfig); //LauncherUI is launcher scene root
            GameSceneLoadedEvent?.Invoke(gameSceneManager);
        }

        public void InvokeGameSceneUnloaded()
        {
            Debug.Log("GameSceneUnloaded Invoked");
            GameSceneUnloadedEvent?.Invoke();
            
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

    }
}