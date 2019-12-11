using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Abu.Tools
{
    public class AsyncLoader : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;
        [SerializeField] private string sceneName;

        private void Awake()
        {
            progressBar.gameObject.SetActive(false);
        }

        public void LoadScene([CanBeNull] string scene = null)
        {
            progressBar.gameObject.SetActive(true);
            if (scene == null)
                scene = sceneName;
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
        }
    }
}