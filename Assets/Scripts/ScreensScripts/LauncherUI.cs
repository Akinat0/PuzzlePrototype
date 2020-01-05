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
        
        public static event Action PlayLauncherEvent;
        public static event Action<GameSceneManager> GameSceneLoadedEvent;
        public static event Action GameSceneUnloadedEvent;
           
        [SerializeField] private AsyncLoader asyncLoader;
        [SerializeField] private Transform playerEntity;
        [SerializeField] private GameObject background;
        [SerializeField] private float partOfThePlayerOnTheScreen;
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
            
            float playerScale =
                ScreenScaler.ScaleToFillPartOfScreen(  
                    playerEntity.GetComponent<PlayerView>().shape.GetComponent<SpriteRenderer>(),
                    partOfThePlayerOnTheScreen);
            playerEntity.localScale = Vector3.one * playerScale;
        }

        void Play()
        {
            if (asyncLoader.gameObject != null)
                asyncLoader.LoadScene("GameScene", InvokeGameSceneLoaded);
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
        
        public void InvokePlayLauncher()
        {
            Debug.Log("PlayLauncher Invoked");
            HidePlayButton();
            Play();
            PlayLauncherEvent?.Invoke();
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
        
        
    }
}