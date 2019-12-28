using System;
using System.Collections;
using JetBrains.Annotations;
using Puzzle;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Abu.Tools
{
    public class AsyncLoader : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;

        private Action<GameSceneManager> _onSceneLoaded;
        
        private void Awake()
        {
            progressBar.gameObject.SetActive(false);
        }

        public void LoadScene(string scene, Action<GameSceneManager> onSceneLoaded)
        {
            _onSceneLoaded = onSceneLoaded;
            progressBar.gameObject.SetActive(true);
            StartCoroutine(AsyncSceneLoading(scene));
        }

        IEnumerator AsyncSceneLoading(string scene)
        {
            AsyncOperation levelLoader = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

            
            while (!levelLoader.isDone)
            {
                progressBar.value = levelLoader.progress;
                yield return null;
            }
            
            GameSceneManager gameSceneManager = GameSceneManager.Instance;

            if (gameSceneManager == null)
                Debug.LogError("There's no GameManagers in the scene");

            _onSceneLoaded?.Invoke(gameSceneManager);
        }
    }
}