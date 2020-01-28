using System;
using System.Collections;
using Puzzle;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Abu.Tools
{
    public class AsyncLoader : MonoBehaviour
    {
        [SerializeField] private GameObject loadingIndicator;

        private Action<GameSceneManager> _onSceneLoaded;
        
        private void Awake()
        {
            loadingIndicator.gameObject.SetActive(false);
        }

        public void LoadScene(string scene, Action<GameSceneManager> onSceneLoaded)
        {
            _onSceneLoaded = onSceneLoaded;
            StartCoroutine(AsyncSceneLoading(scene));
        }

        IEnumerator AsyncSceneLoading(string scene)
        {
            AsyncOperation levelLoader = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

            loadingIndicator.gameObject.SetActive(true);
            
            while (!levelLoader.isDone)
            {
                //progressBar.value = levelLoader.progress;
                yield return null;
            }
            
            loadingIndicator.gameObject.SetActive(false);
            
            GameSceneManager gameSceneManager = GameSceneManager.Instance;

            if (gameSceneManager == null)
                Debug.LogError("There's no GameManager in the scene");

            _onSceneLoaded?.Invoke(gameSceneManager);
        }
    }
}