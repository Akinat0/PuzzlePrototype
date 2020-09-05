using System;
using System.Collections;
using System.Collections.Generic;
using Abu.Console;
using Puzzle;
using ScreensScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Abu.Tools
{
    public class AsyncLoader : MonoBehaviour
    {
        [SerializeField] private GameObject loadingIndicator;
        [SerializeField] string[] ScenesToPreload = new []{"CommonLevelScene"};

        private Action<GameSceneManager> OnSceneLoaded;

        readonly Dictionary<string, AsyncOperation> LevelLoaders = new Dictionary<string, AsyncOperation>();

        private void Start()
        {
            this.Invoke(PreloadScenes, 1f);

            SceneManager.LoadScene("PuzzleAtlasScene", LoadSceneMode.Additive);
            loadingIndicator.gameObject.SetActive(false);
        }

        public void LoadScene(string scene, Action<GameSceneManager> onSceneLoaded)
        {
            OnSceneLoaded = onSceneLoaded;
            StartCoroutine(AsyncSceneLoading(scene));
        }

        IEnumerator AsyncSceneLoading(string scene)
        {
            AsyncOperation levelLoader = GetLevelLoader(scene);
            levelLoader.priority = Int32.MaxValue;

            loadingIndicator.gameObject.SetActive(true);
            
            while (!levelLoader.isDone)
                yield return null;
            
            loadingIndicator.gameObject.SetActive(false);
            
            GameSceneManager gameSceneManager = GameSceneManager.Instance;

            if (gameSceneManager == null)
                Debug.LogError("There's no GameManager in the scene");

            OnSceneLoaded?.Invoke(gameSceneManager);
            OnSceneLoaded = null;
        }

        AsyncOperation GetLevelLoader(string scene)
        {
            if (LevelLoaders.ContainsKey(scene))
            {
                AsyncOperation loader = LevelLoaders[scene];
                loader.allowSceneActivation = true;
                LevelLoaders.Remove(scene);
                return loader;
            }
            
            return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
        
        void PreloadScenes()
        {
            ScenesToPreload = new[] {"CommonLevelScene"};

            int priority = ScenesToPreload.Length;
            
            foreach (string scene in ScenesToPreload)
            {
                priority--;
                AsyncOperation loader = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                LevelLoaders[scene] = loader;
                loader.allowSceneActivation = false;
                loader.priority = priority;
            }
        }

    }
}