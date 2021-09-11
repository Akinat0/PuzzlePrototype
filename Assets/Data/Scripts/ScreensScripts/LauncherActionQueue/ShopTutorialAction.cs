using Abu.Tools.UI;
using Data.Scripts.ScreensScripts;
using ScreensScripts;
using UnityEngine;

public class ShopTutorialAction : LauncherAction
{
    ShopScreen ShopScreen { get; }
    public ButtonComponent ShopButton { get; }

    RectTransformTutorialHole tutorialHole;
    TutorialOverlayView tutorialOverlay;
    
    public ShopTutorialAction(ShopScreen shopScreen, ButtonComponent shopButton) : base(LauncherActionOrder.Tutorial)
    {
        ShopScreen = shopScreen;
        ShopButton = shopButton;
    }

    public override void Start()
    {
        if (Account.ShopAvailable)
        {
            Pop();
            return;
        }

        Account.ShopAvailable.Value = true;

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
            
            LauncherUI.SelectLevel(Account.LevelConfigs[2]);
            Pop();
        }

        shopItem.BuyButton.OnClick += OnShopItemClick;
    }

    public override void Update()
    {
        base.Update();
        
        tutorialHole?.UpdateRect();
    }
}
