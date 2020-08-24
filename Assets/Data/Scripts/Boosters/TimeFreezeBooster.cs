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

    public override void Apply()
    {
        TimeManager.DefaultTimeScale = 0.87f;
        
        Freeze.gameObject.SetActive(true);
        Freeze.Show();
        
        bool isUsed = false;
        
        void HideFreezeScreen()
        {
            if (isUsed)
                return;

            isUsed = true;
            Freeze.Hide(() => Freeze.gameObject.SetActive(false));
        }

        GameSceneManager.LevelCompletedEvent += HideFreezeScreen;
        GameSceneManager.LevelClosedEvent += HideFreezeScreen;
    }


}
