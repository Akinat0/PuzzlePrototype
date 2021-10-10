using Abu.Tools.UI;
using Data.Scripts.Boosters;
using DG.Tweening;
using ScreensScripts;
using UnityEngine;

public class BoostersToggleComponent : ToggleComponent
{
    const float AnimationDuration = 0.6f;
    
    [SerializeField] RectTransform Content;
    [SerializeField] BoosterView timeFreezeBoosterView; 
    [SerializeField] BoosterView heartBoosterView; 

    BlurOverlayView fade;
    
    public BlurOverlayView Fade
    {
        get
        {
            if (fade == null)
            {
                fade = OverlayView.Create<BlurOverlayView>(RectTransform.parent, RectTransform.GetSiblingIndex());
                fade.BlurColor = new Color(0.7f, 0.7f, 0.7f, 0); 
                fade.OnClick += OnButtonClick;
            }

            return fade;
        }
    }

    protected override void Start()
    {
        base.Start();

        if (!Application.isPlaying)
            return;
        
        timeFreezeBoosterView.Initialize(Account.GetBooster<TimeFreezeBooster>());
        heartBoosterView.Initialize(Account.GetBooster<HeartBooster>());
        //add here other boosters initialization

        RegisterTutorialValues();

        SetActive(Tutorials.BoosterTutorial.State == TutorialState.Started 
                  || Tutorials.BoosterTutorial.State == TutorialState.Completed);
    }

    void OnEnable()
    {
        ToggleValueChanged += ToggleValueChanged_Handler;
        LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
        LauncherUI.GameEnvironmentUnloadedEvent += GameEnvironmentUnloadedEventHandler;
    }

    void OnDisable()
    {
        ToggleValueChanged -= ToggleValueChanged_Handler;
        LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
        LauncherUI.GameEnvironmentUnloadedEvent -= GameEnvironmentUnloadedEventHandler;
    }

    void ToggleValueChanged_Handler(bool value)
    {
        if(!Application.IsPlaying(this))
            return;
        
        if (value)
        {
            Fade.ChangePhase(1, AnimationDuration/2);
            RectTransform.DOAnchorPos(Vector2.left * Content.rect.width, AnimationDuration);
        }
        else
        {
            Fade.ChangePhase(0, AnimationDuration);
            RectTransform.DOAnchorPos(Vector2.zero, AnimationDuration);
        }
    }

    void PlayLauncherEvent_Handler(PlayLauncherEventArgs _)
    {
        Interactable = false;
        RectTransform.DOAnchorPos(Vector2.right * RectTransform.rect.width, AnimationDuration);
    }

    void GameEnvironmentUnloadedEventHandler(GameSceneUnloadedArgs _)
    {
        Interactable = true;
        RectTransform.DOAnchorPos(Vector2.zero, AnimationDuration);
    }

    protected virtual void OnDestroy()
    {   
        if(fade != null && fade.gameObject != null)
            Destroy(fade.gameObject);
    }

    void RegisterTutorialValues()
    {
        Tutorials.Register("boosters_toggle", this);
        Tutorials.Register("heart_booster_view", heartBoosterView);
    }
}
