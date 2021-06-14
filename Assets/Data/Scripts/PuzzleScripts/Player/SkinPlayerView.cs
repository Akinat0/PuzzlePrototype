using UnityEngine;

public class SkinPlayerView : PlayerView
{
    [SerializeField, Tooltip("Skin changes by tap")] SkinContainer[] skinContainers;
    [SerializeField, Tooltip("Mask changes by tap")] MaskSkinContainer[] maskContainers;

    int activeSkin = 0;
    int ActiveSkin
    {
        get => activeSkin;
        set
        {
            if (activeSkin == value)
                return;

            activeSkin = value;

            foreach (SkinContainer skinContainer in skinContainers)
                skinContainer.Skin = activeSkin;
            
            foreach (MaskSkinContainer maskContainer in maskContainers)
                maskContainer.Skin = activeSkin;
        }
    }

    protected override void Start()
    {
        base.Start();

        ActiveSkin = 0;
    }

    public override void ChangeSides()
    {
        ActiveSkin = ActiveSkin == 0 ? 1 : 0;
    }

    protected override void RestoreView()
    {
        ActiveSkin = 0;
    }
    
    #if UNITY_EDITOR
    
    public static void SetEditorSkinContainers(SkinPlayerView playerView, SkinContainer[] skins)
    {
        playerView.skinContainers = (SkinContainer[]) skins.Clone();
    }
    
    public static void SetEditorMaskContainers(SkinPlayerView playerView, MaskSkinContainer[] skins)
    {
        playerView.maskContainers = (MaskSkinContainer[]) skins.Clone();
    }
    
    #endif
}
