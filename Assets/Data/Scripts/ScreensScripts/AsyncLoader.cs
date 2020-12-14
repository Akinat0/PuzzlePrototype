using System;
using System.Collections;
using Puzzle;
using UnityEngine;

namespace Abu.Tools
{
    public class AsyncLoader : MonoBehaviour
    {
        [SerializeField] private GameObject loadingIndicator;

        private Action<GameSceneManager> OnEnvironmentLoaded;
        
        private void Awake()
        {
            loadingIndicator.gameObject.SetActive(false);
        }

        public void LoadGameEnvironment(string environmentName, Action<GameSceneManager> complete)
        {
            OnEnvironmentLoaded = complete;
            StartCoroutine(AsyncEnvironmentLoading($"LevelEnvironments/{environmentName}"));
        }

        IEnumerator AsyncEnvironmentLoading(string scene)
        {
            ResourceRequest request = Resources.LoadAsync<GameObject>(scene);
            
            loadingIndicator.gameObject.SetActive(true);
            
            while (!request.isDone)
            {
                //progressBar.value = levelLoader.progress;
                yield return null;
            }
            
            loadingIndicator.gameObject.SetActive(false);

            GameObject gameEnvironment = Instantiate(request.asset as GameObject);

            GameSceneManager gameSceneManager = gameEnvironment.GetComponent<GameSceneManager>();

            if (gameSceneManager == null)
                Debug.LogError("There's no GameManager in the environment");

            OnEnvironmentLoaded?.Invoke(gameSceneManager);
        }
    }
}