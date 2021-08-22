using System.Collections;
using Abu.Tools;
using Abu.Tools.UI;
using ScreensScripts;
using UnityEngine;

public class FirstStartTutorialAction : LauncherAction
{
    public FirstStartTutorialAction() : base(LauncherActionOrder.Tutorial) { }

    public override void Start()
    {
        if (Account.TutorialCompleted)
        {
            Pop();
            return;
        }

        CoroutineHelper.StartRoutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        SoundManager.Instance.SetVolume(0);

        yield return new WaitForSeconds(0.1f);
        
        OverlayView overlay = OverlayView.Create<BlurOverlayView>(LauncherUI.Instance.UiManager.Root,
            LauncherUI.Instance.UiManager.Root.childCount, OverlayView.RaycastTargetMode.Never);
        
        overlay.Phase = 0;
        overlay.ChangePhase(1, 0.2f);
        
        yield return new WaitForSeconds(0.2f);

        LauncherUI.GameEnvironmentUnloadedEvent += GameEnvironmentUnloadedEvent_Handler;
        LauncherUI.PlayLevel(Account.GetLevel("Tutorial"));
        
        overlay.ChangePhase(0, 0.2f, () => overlay.Destroy());
        
        yield return new WaitForSeconds(0.2f);
        
        SoundManager.Instance.SetVolume(1);
    }

    void GameEnvironmentUnloadedEvent_Handler(GameSceneUnloadedArgs args)
    {
        LauncherUI.GameEnvironmentUnloadedEvent -= GameEnvironmentUnloadedEvent_Handler;
        Pop();
    }
}
