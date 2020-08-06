using Abu.Tools.UI;
using Data.Scripts.Boosters;
using DG.Tweening;
using UnityEngine;

public class BoostersToggleComponent : ToggleComponent
{
    const float AnimationDuration = 0.6f;
    
    [SerializeField] RectTransform Content;
    [SerializeField] BoosterView timeFreezeBoosterView; 

    BlurOverlayView fade;

    public BlurOverlayView Fade
    {
        get
        {
            if (fade == null)
            {
                fade = OverlayView.Create<BlurOverlayView>(RectTransform.parent, RectTransform.GetSiblingIndex());
                fade.BlurColor = new Color(0.7f, 0.7f, 0.7f, 1);
                fade.OnClick += OnButtonClick;
            }

            return fade;
        }
    }

    protected virtual void Start()
    {
        timeFreezeBoosterView.Initialize(Account.GetBooster<TimeFreezeBooster>());
        //TODO add here other boosters initialization
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
            if(Mathf.Approximately(Fade.Phase, 0))
                Fade.RecreateBlurTexture();
            
            Fade.ChangePhase(1, AnimationDuration/2);
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
        if(fade != null && fade.gameObject != null)
            Destroy(fade.gameObject);
    }
}
