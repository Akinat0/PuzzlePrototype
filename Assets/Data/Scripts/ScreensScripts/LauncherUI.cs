﻿using System;
using UnityEngine;
using Abu.Tools;
using Abu.Tools.UI;
using Puzzle;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace ScreensScripts
{
    public class LauncherUI : MonoBehaviour
    {
        public static LauncherUI Instance;
        
        public static event Action<PlayLauncherEventArgs> PlayLauncherEvent;
        public static event Action<GameSceneManager> GameEnvironmentLoadedEvent;
        public static event Action<GameSceneUnloadedArgs> GameEnvironmentUnloadedEvent;
        public static event Action<LevelChangedEventArgs> LevelChangedEvent;
        public static event Action<ShowCollectionEventArgs> ShowCollectionEvent;
        public static event Action<CloseCollectionEventArgs> CloseCollectionEvent;
        public static event Action<Achievement> AchievementReceived;
        public static event Action<Achievement> ShowAchievementsScreenEvent;
        public static event Action<LevelConfig> PlayLevelEvent; 
        public static event Action<LevelConfig> SelectLevelEvent; 


        [SerializeField] AsyncLoader asyncLoader;
        [SerializeField] MainMenuUIManager uiManager;
        [SerializeField] MainCameraComponent mainCamera;
        [SerializeField] LauncherActionQueue launcherActionQueue;

        public MainMenuUIManager UiManager => uiManager;
        public MainCameraComponent MainCamera => mainCamera;
        public LauncherActionQueue ActionQueue => launcherActionQueue;

        public LevelConfig LevelConfig => levelConfig;
        
        GameSceneManager GameSceneManager;

        LevelConfig levelConfig;

        LevelRootView actualLevelRootView;
        Transform playerEntity;
        
        public TextGroupComponent LauncherTextGroup { get; private set; }
        
        void Awake()
        {
            Instance = this;
            SceneManager.LoadScene("PuzzleAtlasScene", LoadSceneMode.Additive);
            LauncherTextGroup = TextGroupComponent.AttachTo(gameObject);
        }

        void Start()
        {
            Tutorials.TryStartTutorials();
        }

        public static void PlayLevel(LevelConfig levelConfig) => Instance.InvokePlayLevel(levelConfig); 
        public static void SelectLevel(LevelConfig levelConfig) => Instance.InvokeSelectLevel(levelConfig); 
        
        
        void LoadLevel(LevelConfig config)
        {
            if (asyncLoader.gameObject != null && config != null && !string.IsNullOrEmpty(config.SceneID))
                asyncLoader.LoadGameEnvironment(config.SceneID, InvokeGameEnvironmentLoaded);
        }

        public void InvokePlayLauncher(PlayLauncherEventArgs args)
        {
            Debug.Log("PlayLauncher Invoked");
            levelConfig = args.LevelConfig;
            actualLevelRootView = args.LevelRootView;
            PlayLauncherEvent?.Invoke(args);
            LoadLevel(args.LevelConfig);
        }
        
        public void InvokeGameEnvironmentLoaded(GameSceneManager gameSceneManager)
        {
            Debug.Log("GameEnvironmentLoaded Invoked");
            gameSceneManager.SetupScene(playerEntity.gameObject, levelConfig, actualLevelRootView); //LauncherUI is launcher scene root
            GameEnvironmentLoadedEvent?.Invoke(gameSceneManager);
        }

        public void InvokeGameEnvironmentUnloaded(GameSceneUnloadedArgs args)
        {
            Debug.Log("GameEnvironmentUnloaded Invoked");
            GameEnvironmentUnloadedEvent?.Invoke(args);

            //Unpause game anyway
            TimeManager.DefaultTimeScale = 1;
            TimeManager.Unpause();
        }

        public void InvokeLevelChanged(LevelChangedEventArgs args)
        {
            Debug.Log("LevelChanged Invoked");
            playerEntity = args.PlayerView.transform;
            levelConfig = args.LevelConfig;
            LevelChangedEvent?.Invoke(args);
        }

        public void InvokeShowCollection(ShowCollectionEventArgs args)
        {
            Debug.Log("LevelChanged Invoked");
            ShowCollectionEvent?.Invoke(args);
        }

        public void InvokeCloseCollection(CloseCollectionEventArgs args)
        {
            Debug.Log("CloseCollection Invoked");
            
            if(args.PlayerView != null)
                playerEntity = args.PlayerView.transform;
            
            CloseCollectionEvent?.Invoke(args);
        }
        
        public void InvokeAchievementReceived(Achievement achievement)
        {
            Debug.Log($"Achievement {achievement.Name} received Invoked");
            AchievementReceived?.Invoke(achievement);
        }

        public void InvokeShowAchievementScreen(Achievement achievement)
        {
            Debug.Log($"Show achievements screen Invoked");
            ShowAchievementsScreenEvent?.Invoke(achievement);
        }

        void InvokePlayLevel(LevelConfig levelConfig)
        {
            Debug.Log($"Play level {levelConfig.Name} Invoked");
            PlayLevelEvent?.Invoke(levelConfig);
        }

        void InvokeSelectLevel(LevelConfig levelConfig)
        {
            Debug.Log($"Select level {levelConfig.Name} Invoked");
            SelectLevelEvent?.Invoke(levelConfig);
        }

    }
}