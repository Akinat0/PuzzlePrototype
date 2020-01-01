using System;
using UnityEngine;
using Abu.Tools;
using Puzzle;

namespace ScreensScripts
{

    public class LauncherUI : MonoBehaviour
    {

        public static event Action PlayLauncherEvent;
        public static event Action<GameSceneManager> GameSceneLoadedEvent;
        
        [SerializeField] private AsyncLoader asyncLoader;
        [SerializeField] private Transform playerEntity;
        [SerializeField] private GameObject background;
        [SerializeField] private float partOfThePlayerOnTheScreen;
        [SerializeField] private Canvas launcherCanvas;

        // ReSharper disable once InconsistentNaming
        private GameSceneManager GameSceneManager;
        
        void Play()
        {
            if (asyncLoader.gameObject != null)
                asyncLoader.LoadScene("GameScene", InvokeGameSceneLoaded);
        }

        void HideUi()
        {
            launcherCanvas.gameObject.SetActive(false);
            background.SetActive(false);
        }
        
        private void Start()
        {
            float playerScale =
                ScreenScaler.ScaleToFillPartOfScreen(  
                    playerEntity.GetComponent<PlayerView>().shape.GetComponent<SpriteRenderer>(),
                    partOfThePlayerOnTheScreen);
            playerEntity.localScale = Vector3.one * playerScale;
        }

        public void InvokePlayLauncher()
        {
            Debug.Log("PlayLauncher Invoked");
            Play();
            PlayLauncherEvent?.Invoke();
        }
        
        void InvokeGameSceneLoaded(GameSceneManager gameSceneManager)
        {
            HideUi();
            Debug.Log("GameSceneLoaded Invoked  ");
            gameSceneManager.SetupScene(playerEntity.gameObject, background, gameObject); //LauncherUI is game root
            GameSceneLoadedEvent?.Invoke(gameSceneManager);
        }
    }
}