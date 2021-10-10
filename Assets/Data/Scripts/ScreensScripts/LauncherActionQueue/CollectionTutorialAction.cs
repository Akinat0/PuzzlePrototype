using System;
using Abu.Tools.UI;
using Data.Scripts.ScreensScripts;
using ScreensScripts;
using UnityEngine;

public class CollectionTutorialAction : TutorialAction
{
    [TutorialProperty("collection_button")]
    public ButtonComponent CollectionButton { get; set; }
    
    [TutorialProperty("collection_screen")]
    public CollectionScreen CollectionScreen { get; set; }
    
    [TutorialProperty("can_start_collection_tutorial")]
    public Func<bool> CanStartPredicate { get; set; }
    
    TutorialOverlayView overlayView;
    RectTransformTutorialHole currentHole;

    RectTransform Root => LauncherUI.Instance.UiManager.Root;
    
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
        CollectionItem target = Account.GetCollectionItem("Cool Guy");
        
        ButtonComponent collectionItemButton = CollectionScreen.GetItemButton(target);

        currentHole = new RectTransformTutorialHole(collectionItemButton.Content);
        overlayView.AddHole(currentHole);
        overlayView.ChangePhase(0.975f, 0.3f);

        void CollectionItemClicked()
        {
            CollectionScreen.IsScrollable = true;
            collectionItemButton.OnClick -= CollectionItemClicked;
            overlayView.ChangePhase(0, 0.25f);
            overlayView.RemoveHole(currentHole);
            overlayView.Destroy();
            Pop();
        }

        CollectionScreen.IsScrollable = false;
        collectionItemButton.OnClick += CollectionItemClicked;
    }

}
