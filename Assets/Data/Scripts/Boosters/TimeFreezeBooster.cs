using System;
using System.Runtime.InteropServices;
using Abu.Tools.UI;
using Puzzle;
using ScreensScripts;

public class TimeFreezeBooster : Booster
{
    public override string Name => "Time Freeze";

    FreezeOverlayView freeze;

    FreezeOverlayView Freeze
    {
        get
        {
            if(freeze == null)
                freeze = OverlayView.Create<FreezeOverlayView>(LauncherUI.Instance.UiManager.Root, 0);
            
            return freeze;
        }
    }

    Action HideFreezeScreen; 

    protected override void Apply()
    {
        TimeManager.DefaultTimeScale = 0.87f;
        
        Freeze.gameObject.SetActive(true);
        Freeze.Show();

        HideFreezeScreen = () => Freeze.Hide(() => Freeze.gameObject.SetActive(false));

        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }

    void LevelCompletedEvent_Handler(LevelCompletedEventArgs _)
    {
        InvokeHideFreezeScreen();
    }

    void LevelClosedEvent_Handler()
    {
        InvokeHideFreezeScreen();
    }
    
    void ResetLevelEvent_Handler()
    {
        InvokeHideFreezeScreen();
    }

    void InvokeHideFreezeScreen()
    {
        if(HideFreezeScreen == null)
            return;

        Action action = HideFreezeScreen;
        HideFreezeScreen = null;
        
        action?.Invoke();
    }
}
