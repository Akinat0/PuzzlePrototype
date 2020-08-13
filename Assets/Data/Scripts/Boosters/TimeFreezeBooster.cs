
using Puzzle;
using ScreensScripts;
using UnityEngine;

public class TimeFreezeBooster : Booster
{
    public override string Name => "Time Freeze";


    ScreenFreezeEffect screenFreeze;

    ScreenFreezeEffect ScreenFreeze
    {
        get
        {
            if(screenFreeze == null)
                screenFreeze = ScreenFreezeEffect.Create(LauncherUI.Instance.transform);
            
            return screenFreeze;
        }
    }

    public override void Apply()
    {
        TimeManager.DefaultTimeScale = 0.87f;
        
        ScreenFreeze.gameObject.SetActive(true);
        ScreenFreeze.Show();
        
        bool isUsed = false;
        
        void HideFreezeScreen()
        {
            if (isUsed)
                return;

            isUsed = true;
            ScreenFreeze.Hide(() => ScreenFreeze.gameObject.SetActive(false));
        }

        GameSceneManager.LevelCompletedEvent += HideFreezeScreen;
        GameSceneManager.LevelClosedEvent += HideFreezeScreen;
    }


}
