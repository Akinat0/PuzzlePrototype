using Abu.Tools.UI;
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

    protected override void Apply()
    {
        TimeManager.DefaultTimeScale = 0.87f;
        
        Freeze.gameObject.SetActive(true);
        Freeze.Show();
    }

    public override void Release()
    {
        TimeManager.DefaultTimeScale = 1;
        Freeze.Hide(() => Freeze.gameObject.SetActive(false));
    }
    
}
