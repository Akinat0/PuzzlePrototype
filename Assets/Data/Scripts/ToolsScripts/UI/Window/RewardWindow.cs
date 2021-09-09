using System;
using Abu.Tools.UI;
using DG.Tweening;
using UnityEngine;

public class RewardWindow : Window
{
    public static RewardWindow Create(Reward reward, Action onSuccess = null)
    {
        RewardWindow window = Instantiate(Resources.Load<RewardWindow>("UI/RewardWindow"), GetRoot());

        window.Initialize(reward, onSuccess);
        
        return window;
    }

    Reward Reward { get; set; }
    RewardShine Shine { get; set; }

    void Initialize(Reward reward, Action onSuccess)
    {
        Reward = reward;
        
        RectTransform.SetAsLastSibling();

        Shine = RewardShine.Create(ContentContainer, reward.Rarity);
        Shine.RectTransform.localScale = Vector3.zero;
        
        Reward.CreateView(Shine.RectTransform);
        Shine.RectTransform.DOScale(Vector3.one, 0.2f);
        
        Title.Text = "Reward";
        
        BlurOverlayView overlay = OverlayView.Create<BlurOverlayView>(GetRoot(), RectTransform.GetSiblingIndex());

        void OnClick()
        {
            onSuccess?.Invoke();
            Hide();
        }
        
        Overlay = overlay;
        Overlay.OnClick += OnClick;
        
        OkButton.Text = "Ok";
        OkButton.OnClick += OnClick;

        RectTransform.localScale = Vector2.zero;
        Show();
    }

    void OnDestroy()
    {
        //Stop shine routine
        Shine.DOKill();
        Shine.Destroy();
    }
}
