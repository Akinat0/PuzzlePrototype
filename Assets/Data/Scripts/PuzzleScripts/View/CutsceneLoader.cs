﻿using System;
using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using Abu.Tools.SceneTransition;
using DG.Tweening;
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
                Utility.WaitUntil(() => loader.isDone,
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

    private void CutsceneStartedEvent_Handler(CutsceneEventArgs _args)
    {
        Scene scene = SceneManager.GetSceneByName(_args.SceneID);
        
        ISceneTransition sceneTransition = SceneTransitions.Create(_args.SceneTransition);

        
        sceneTransition.InTransition(0.6f, () =>
        {
            if (!scene.isLoaded)
            {
                StartCoroutine(Utility.WaitUntil(
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
            
            sceneTransition.Unload();
        });
    }
    
    private void CutsceneEndedEvent_Handler(CutsceneEventArgs _args)
    {
        Scene scene = SceneManager.GetSceneByName(_args.SceneID);

        ISceneTransition sceneTransition = SceneTransitions.Create(_args.SceneTransition);
        
        sceneTransition.OutTransition(0.6f, () => sceneTransition.Unload());

        if (scene.isLoaded)
            foreach (GameObject root in scene.GetRootGameObjects())
                root.SetActive(false);
    }
    
}
