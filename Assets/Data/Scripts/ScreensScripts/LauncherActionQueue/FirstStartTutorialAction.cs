using System.Collections;
using Abu.Tools;
using Abu.Tools.UI;
using Data.Scripts.Tools.Input;
using ScreensScripts;
using UnityEngine;

public class FirstStartTutorialAction : TutorialAction
{
    public override void Start()
    {
        CoroutineHelper.StartRoutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        SoundManager.Instance.SetVolume(0);
        MobileInput.Condition = false;

        yield return new WaitForSeconds(0.1f);
        
        OverlayView overlay = OverlayView.Create<BlurOverlayView>(LauncherUI.Instance.UiManager.Root,
            LauncherUI.Instance.UiManager.Root.childCount, OverlayView.RaycastTargetMode.Always);
        
        overlay.Phase = 0;
        overlay.ChangePhase(1, 0.2f);
        
        yield return new WaitForSeconds(1f);

        MobileInput.Condition = true;
        SoundManager.Instance.SetVolume(1);
        
        LevelConfig level = Account.GetLevel("Tutorial");
        
        LauncherUI.SelectLevel(level);
        LauncherUI.PlayLevel(level);
        
        overlay.ChangePhase(0, 0.2f, () => overlay.Destroy());
        
        yield return new WaitForSeconds(0.2f);
        
        Pop();
    }
}
