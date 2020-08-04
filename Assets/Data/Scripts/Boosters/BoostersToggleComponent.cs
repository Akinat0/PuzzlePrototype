using Abu.Tools.UI;
using Data.Scripts.Boosters;
using DG.Tweening;
using UnityEngine;

public class BoostersToggleComponent : ToggleComponent
{
    const float AnimationDuration = 0.6f;
    
    [SerializeField] RectTransform Content;
    [SerializeField] BoosterView timeFreezeBoosterView; 

    FadeOverlayView fade;

    public FadeOverlayView Fade
    {
        get
        {
            if (fade == null)
            {
                fade = OverlayView.Create<FadeOverlayView>(RectTransform.parent, RectTransform.GetSiblingIndex());
                fade.OnClick += () => IsOn = false;
            }

            return fade;
        }
    }

    protected virtual void Start()
    {
        timeFreezeBoosterView.Initialize(Account.GetBooster<TimeFreezeBooster>());
        
    }


    void OnEnable()
    {
        ToggleValueChanged += ToggleValueChanged_Handler;
    }

    void OnDisable()
    {
        ToggleValueChanged -= ToggleValueChanged_Handler;
    }

    void ToggleValueChanged_Handler(bool value)
    {
        if(!Application.IsPlaying(this))
            return;
        
        if (value)
        {
            Fade.ChangePhase(0.5f, AnimationDuration/2);
            RectTransform.DOAnchorPos(Vector2.left * Content.rect.width, AnimationDuration);
        }
        else
        {
            Fade.ChangePhase(0, AnimationDuration);
            RectTransform.DOAnchorPos(Vector2.zero, AnimationDuration);
        }
    }

    protected virtual void OnDestroy()
    {
        Destroy(Fade);
    }
}
