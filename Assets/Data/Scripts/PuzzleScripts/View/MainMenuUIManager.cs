using System;
using Abu.Tools.UI;
using ScreensScripts;
using UnityEngine;

namespace Puzzle
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [field: SerializeField] public WalletComponent Wallet { get; private set; }

        void OnEnable()
        {
            LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
            LauncherUI.GameSceneUnloadedEvent += GameSceneUnloadedEvent_Handler;
        }
        
        void OnDisable()
        {
            LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
            LauncherUI.GameSceneUnloadedEvent -= GameSceneUnloadedEvent_Handler;
        }

        void PlayLauncherEvent_Handler(PlayLauncherEventArgs _)
        {
            Wallet.Alpha = 0;
        }

        void GameSceneUnloadedEvent_Handler()
        {
            Wallet.Alpha = 1;
        }
    }
}