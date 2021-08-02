using Abu.Tools.UI;
using ScreensScripts;
using UnityEngine;

public class LevelSelectorFadeProcessor : MonoBehaviour
{
    const float LevelUnlockedPhase = 0;
    const float LevelLockedPhase = 0.3f;

    FadeOverlayView fadeOverlay;

    FadeOverlayView FadeOverlay =>
        fadeOverlay 
            ? fadeOverlay 
            : fadeOverlay = OverlayView.Create<FadeOverlayView>(LauncherUI.Instance.UiManager.Root, 0, OverlayView.RaycastTargetMode.Never);
    
    public void ProcessIndex(int index, LevelConfig[] selection)
    {
        LevelConfig current = selection[index];

        FadeOverlay.Phase = current.CanPlayLevel ? LevelUnlockedPhase : LevelLockedPhase;
    }

    public void ProcessOffset(float offset, int index, LevelConfig[] selection, LevelConfig nextLevel)
    {
        if(nextLevel == null)
            return;
        
        LevelConfig current = selection[index];
        
        float startPhase = current.CanPlayLevel ? LevelUnlockedPhase : LevelLockedPhase;
        float targetPhase = nextLevel.CanPlayLevel ? LevelUnlockedPhase : LevelLockedPhase;

        float phase = Mathf.Abs(offset - index);
        FadeOverlay.Phase = Mathf.Lerp(startPhase, targetPhase, phase);
    }
}
