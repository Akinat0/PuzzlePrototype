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
        [SerializeField] private GameObject background;
        [SerializeField] private Canvas launcherCanvas;
        [SerializeField] private GameObject playButton;
        
        // ReSharper disable once InconsistentNaming
        private GameSceneManager GameSceneManager;

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

        void HidePlayButton()
        {
            playButton.SetActive(false);
        }
        
        void HideUi()
        {
            playButton.SetActive(true);
            launcherCanvas.gameObject.SetActive(false);
            background.SetActive(false);
        }

        void ShowUi()
        {
            launcherCanvas.gameObject.SetActive(true);
            background.SetActive(true);
        }
        
        public void InvokePlayLauncher(PlayLauncherEventArgs _Args)
        {
            Debug.Log("PlayLauncher Invoked");
            HidePlayButton();
            PlayLevel(_Args.LevelConfig);
            PlayLauncherEvent?.Invoke(_Args);
        }
        
        public void InvokeGameSceneLoaded(GameSceneManager gameSceneManager)
        {
            HideUi();
            Debug.Log("GameSceneLoaded Invoked");
            gameSceneManager.SetupScene(playerEntity.gameObject, background, gameObject); //LauncherUI is launcher scene root
            GameSceneLoadedEvent?.Invoke(gameSceneManager);
        }

        public void InvokeGameSceneUnloaded()
        {
            Debug.Log("GameSceneUnloaded Invoked");
            ShowUi();
            GameSceneUnloadedEvent?.Invoke();
        }

        public void InvokeLevelChanged(LevelChangedEventArgs _Args)
        {
            Debug.Log("LevelChanged Invoked");
            playerEntity = _Args.PlayerView.transform;
            LevelChangedEvent?.Invoke(_Args);
        }
        
    }
}