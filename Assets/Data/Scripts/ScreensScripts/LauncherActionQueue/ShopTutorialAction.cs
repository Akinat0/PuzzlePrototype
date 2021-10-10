using Abu.Tools.UI;
using Data.Scripts.ScreensScripts;
using ScreensScripts;
using UnityEngine;

public class ShopTutorialAction : TutorialAction
{
    [TutorialProperty("shop_screen")]
    public ShopScreen ShopScreen { get; set; }
    
    [TutorialProperty("shop_button")]
    public ButtonComponent ShopButton { get; set; }
    
    [TutorialProperty("main_menu_close_button")]
    public ButtonComponent CloseButton { get; set; }

    RectTransformTutorialHole tutorialHole;
    TutorialOverlayView tutorialOverlay;
    
    public override void Start()
    {
        if (Tutorials.ShopTutorial.State == TutorialState.Started)
        {
            StartShopButtonTutorial();
        }
        else
        {
            bool secondLevelClosed = false;
        
            void TryStartTutorial(GameSceneUnloadedArgs args)
            {
                if (args.LevelConfig != Account.LevelConfigs[1])
                    return;
            
                secondLevelClosed = true;
                LauncherUI.GameEnvironmentUnloadedEvent -= TryStartTutorial;
            }
        
            LauncherUI.GameEnvironmentUnloadedEvent += TryStartTutorial;
            
            bool CanStartPredicate() => secondLevelClosed; 
        
            StartCoroutine(Coroutines.WaitUntil(CanStartPredicate, StartShopButtonTutorial));
        }
    }

    void StartShopButtonTutorial()
    {
        Transform root = LauncherUI.Instance.UiManager.Root;
        
        tutorialOverlay = OverlayView.Create<TutorialOverlayView>(root,
            root.childCount, OverlayView.RaycastTargetMode.Never);

        tutorialOverlay.Color = new Color(0.66f, 0.66f, 0.66f, 0.35f);

        tutorialHole = new RectTransformTutorialHole(ShopButton.Content);
        
        tutorialOverlay.AddHole(tutorialHole);
        tutorialOverlay.ChangePhase(0.975f, 0.5f);

        void OnShopClick()
        {
            ShopButton.OnClick -= OnShopClick;
            tutorialOverlay.Phase = 0;
            tutorialOverlay.RemoveHole(tutorialHole);
            tutorialHole = null;
            
            LauncherUI.SelectLevel(Account.LevelConfigs[2]);
            
            StartShopScreenTutorial();
        }

        ShopButton.OnClick += OnShopClick;
    }

    void StartShopScreenTutorial()
    {
        Tier targetTier = Account.GetTier("free_heart_booster");
        
        ShopScreen.IsScrollable = false;
        ShopScreen.FocusOn(targetTier);
        
        ShopItem shopItem = ShopScreen.GetShopItem(targetTier);
        tutorialHole = new RectTransformTutorialHole(shopItem.RectTransform);
        tutorialOverlay.AddHole(tutorialHole);
        tutorialOverlay.ChangePhase(0.975f, 0.5f);

        void OnShopItemClick()
        {
            shopItem.BuyButton.OnClick -= OnShopItemClick;
            ShopScreen.IsScrollable = true;
            tutorialOverlay.ChangePhase(0, 0.5f, () => tutorialOverlay.Destroy());

            CompleteTutorial();
        }

        shopItem.BuyButton.OnClick += OnShopItemClick;
    }

    void CompleteTutorial()
    {
        void OnCloseClick()
        {
            CloseButton.OnClick -= OnCloseClick;
            ShopScreen.OnOverlayClick -= OnCloseClick;
            Pop();
        }

        CloseButton.OnClick += OnCloseClick;
        ShopScreen.OnOverlayClick += OnCloseClick;
    }

    public override void Update()
    {
        base.Update();
        
        tutorialHole?.UpdateRect();
    }
}
