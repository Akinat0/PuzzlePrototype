using System;
using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using Puzzle;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    [SerializeField] private string[] Scenes;

    private void Start()
    {
        //Preload scenes
        foreach (string scene in Scenes)
        {
            AsyncOperation loader = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            StartCoroutine(
                Extensions.WaitUntil(() => loader.isDone,
                    () =>
                    {
                        foreach (var root in SceneManager.GetSceneByName(scene).GetRootGameObjects())
                            root.SetActive(false);
                    }
                ));
        }
    }

    private void OnDestroy()
    {
        foreach (string scene in Scenes)
            SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }

    private void OnEnable()
    {
        GameSceneManager.CutsceneStartedEvent += CutsceneStartedEvent_Handler;
        GameSceneManager.CutsceneEndedEvent += CutsceneEndedEvent_Handler;
    }
    
    private void OnDisable()
    {
        GameSceneManager.CutsceneStartedEvent -= CutsceneStartedEvent_Handler;
        GameSceneManager.CutsceneEndedEvent -= CutsceneEndedEvent_Handler;
    }

    private void CutsceneStartedEvent_Handler(string sceneID)
    {
        Scene scene = SceneManager.GetSceneByName(sceneID);

        if (!scene.isLoaded)
        {
            StartCoroutine(Extensions.WaitUntil(
                () => scene.isLoaded,
                () =>
                {
                    foreach (GameObject root in scene.GetRootGameObjects())
                        root.SetActive(true);
                }));
        }
        else
        {
            foreach (GameObject root in scene.GetRootGameObjects())
                root.SetActive(true);
        }
    }
    
    private void CutsceneEndedEvent_Handler(string sceneID)
    {
        Scene scene = SceneManager.GetSceneByName(sceneID);

        if (scene.isLoaded)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
                root.SetActive(false);
        }
    }
    
}
