using System;
using Abu.Tools.UI;
using ScreensScripts;
using UnityEngine;

public class CollectionTutorialAction : LauncherAction
{
    ButtonComponent CollectionButton { get; }
    Func<CollectionItem, ButtonComponent> CollectionItemGetter { get; }
    Func<bool> CanStartPredicate { get; }

    TutorialOverlayView overlayView;

    RectTransformTutorialHole currentHole;

    RectTransform Root => LauncherUI.Instance.UiManager.Root;
    
    public CollectionTutorialAction(ButtonComponent collectionButton, Func<CollectionItem, ButtonComponent> collectionItemGetter, Func<bool> canStartPredicate) : base(LauncherActionOrder.Tutorial)
    {
        CollectionButton = collectionButton;
        CollectionItemGetter = collectionItemGetter;
        CanStartPredicate = canStartPredicate;
    }

    public override void Start()
    {
        StartCoroutine(Coroutines.WaitUntil(CanStartPredicate, CollectionButtonTutorial));
    }

    public override void Update()
    {
        base.Update();
        
        currentHole?.UpdateRect();
    }

    void CollectionButtonTutorial()
    {
        overlayView = OverlayView.Create<TutorialOverlayView>(Root, Root.childCount, OverlayView.RaycastTargetMode.Never);
        overlayView.Color = new Color(0.66f, 0.66f, 0.66f, 0.35f);

        currentHole = new RectTransformTutorialHole(CollectionButton.Content);
        overlayView.AddHole(currentHole);
        
        overlayView.ChangePhase(0.975f, 0.5f);
        
        void CollectionButtonClicked()
        {
            CollectionButton.OnClick -= CollectionButtonClicked;
            overlayView.ChangePhase(0, 0.5f);
            overlayView.RemoveHole(currentHole);
            CollectionItemTutorial();
        }
        
        CollectionButton.OnClick += CollectionButtonClicked;
    }

    void CollectionItemTutorial()
    {
        ButtonComponent collectionItemButton = CollectionItemGetter(Account.GetCollectionItem("Cool Guy"));
        
        currentHole = new RectTransformTutorialHole(collectionItemButton.Content);
        overlayView.AddHole(currentHole);
        overlayView.ChangePhase(0.975f, 0.3f);

        void CollectionItemClicked()
        {
            collectionItemButton.OnClick -= CollectionItemClicked;
            overlayView.ChangePhase(0, 0.25f);
            overlayView.RemoveHole(currentHole);
            overlayView.Destroy();
            Pop();
        }

        collectionItemButton.OnClick += CollectionItemClicked;
    }

}
