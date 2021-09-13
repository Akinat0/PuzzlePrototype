using Abu.Tools.UI;
using Data.Scripts.Boosters;
using ScreensScripts;
using UnityEngine;

public class BoosterTutorialAction : LauncherAction
{
    public BoostersToggleComponent BoostersToggle { get; }
    public BoosterView HeartBoosterView { get; }

    TutorialOverlayView tutorialOverlay;
    RectTransformTutorialHole tutorialHole;

    public BoosterTutorialAction(BoostersToggleComponent boostersToggle, BoosterView heartBoosterView) : base(LauncherActionOrder.Tutorial)
    {
        BoostersToggle = boostersToggle;
        HeartBoosterView = heartBoosterView;
    }

    public override void Start()
    {
        if (Account.BoostersAvailable)
        {
            Pop();
            return;
        }

        Account.BoostersAvailable.Value = true;

        RectTransform root = LauncherUI.Instance.UiManager.Root;

        tutorialOverlay = OverlayView.Create<TutorialOverlayView>(root, root.childCount, OverlayView.RaycastTargetMode.Never);
        tutorialOverlay.Color = new Color(0f, 0f, 0f, 0.4f);
        tutorialOverlay.Phase = 0;
        
        StartBoosterToggleTutorial();
    }

    public override void Update()
    {
        base.Update();
        
        tutorialHole?.UpdateRect();
    }

    void StartBoosterToggleTutorial()
    {
        tutorialHole = new RectTransformTutorialHole(BoostersToggle.RectTransform);
        tutorialOverlay.AddHole(tutorialHole);
        
        tutorialOverlay.ChangePhase(0.975f, 0.5f);

        void OnBoostersToggleClick()
        {
            tutorialOverlay.Phase = 0;
            tutorialOverlay.RemoveHole(tutorialHole);
            tutorialHole = null;
        
            BoostersToggle.OnClick -= OnBoostersToggleClick;
            StartBoosterViewTutorial();
        }

        BoostersToggle.OnClick += OnBoostersToggleClick;
    }

    void StartBoosterViewTutorial()
    {
        tutorialHole = new RectTransformTutorialHole(HeartBoosterView.RectTransform);
        tutorialOverlay.AddHole(tutorialHole);
        
        tutorialOverlay.ChangePhase(0.975f, 0.5f);
        
        void OnBoosterViewClick()
        {
            tutorialOverlay.ChangePhase(0, 0.5f, () => tutorialOverlay.Destroy());
            tutorialHole = null;
            
            HeartBoosterView.OnClick -= OnBoosterViewClick;
            Pop();
        }
        
        HeartBoosterView.OnClick += OnBoosterViewClick;
    }
}
