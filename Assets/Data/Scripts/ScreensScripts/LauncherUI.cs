using System;
using System.Diagnostics;
using UnityEngine;
using Abu.Tools;
using Puzzle;
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
         
           
        [SerializeField] private AsyncLoader asyncLoader;
        [SerializeField] private Transform playerEntity;
        [SerializeField] private GameObject backgroundContainer;
        [SerializeField] private Canvas launcherCanvas;

        // ReSharper disable once InconsistentNaming
        private GameSceneManager GameSceneManager;

        private LevelConfig _levelConfig;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Screen.fullScreen = true;
        }

        void PlayLevel(LevelConfig _Config)
        {
            if (asyncLoader.gameObject != null && _Config != null && !string.IsNullOrEmpty(_Config.SceneID))
                asyncLoader.LoadScene(_Config.SceneID, InvokeGameSceneLoaded);
        }

        public void InvokePlayLauncher(PlayLauncherEventArgs _Args)
        {
            Debug.Log("PlayLauncher Invoked");
            _levelConfig = _Args.LevelConfig;
            PlayLevel(_Args.LevelConfig);
            PlayLauncherEvent?.Invoke(_Args);
        }
        
        public void InvokeGameSceneLoaded(GameSceneManager gameSceneManager)
        {
            Debug.Log("GameSceneLoaded Invoked");
            gameSceneManager.SetupScene(playerEntity.gameObject, backgroundContainer, gameObject, _levelConfig.ColorScheme); //LauncherUI is launcher scene root
            GameSceneLoadedEvent?.Invoke(gameSceneManager);
        }

        public void InvokeGameSceneUnloaded()
        {
            Debug.Log("GameSceneUnloaded Invoked");
            GameSceneUnloadedEvent?.Invoke();
            Time.timeScale = 1;
        }

        public void InvokeLevelChanged(LevelChangedEventArgs _Args)
        {
            Debug.Log("LevelChanged Invoked");
            playerEntity = _Args.PlayerView.transform;
            LevelChangedEvent?.Invoke(_Args);
        }
        
    }
}