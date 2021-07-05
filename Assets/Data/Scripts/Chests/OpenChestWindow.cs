using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abu.Tools.GameActionPool;
using Abu.Tools.UI;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OpenChestWindow : Window
{
    [Serializable]
    struct RewardContainers
    {
        public RectTransform[] containers;
        public int Count => containers.Length;
    }
    
    public static OpenChestWindow Create(Reward[] rewards, Rarity rarity, Action onSuccess = null)
    {
        OpenChestWindow window = Instantiate(Resources.Load<OpenChestWindow>("UI/OpenChestWindow"), Root);

        window.Initialize(rewards, rarity, onSuccess);
        
        return window;
    }

    [SerializeField] RectTransform chestSource;
    [SerializeField] RewardContainers[] rewardContainersList;
    
    Animator Animator { get; set; }
    Rarity Rarity { get; set; }
    Reward[] Rewards { get; set; }

    GameActionQueue actionQueue;

    bool animationsCompleted;
    bool chestOpened;
    bool openStarted;
    bool completed;
    
    static readonly int ShowID = Animator.StringToHash("Show");
    static readonly int SpeedID = Animator.StringToHash("Speed");

    void Initialize(Reward[] rewards, Rarity rarity, Action onSuccess)
    {
        Animator = GetComponent<Animator>();
        Rarity = rarity;
        Rewards = rewards;
        
        RectTransform.SetAsLastSibling();

        Title.Text = rarity.ToString();
        
        BlurOverlayView blurOverlay = OverlayView.Create<BlurOverlayView>(Root, RectTransform.GetSiblingIndex());
        blurOverlay.BlurColor = new Color32(135, 135, 135, byte.MaxValue);
        blurOverlay.RaycastMode = BlurOverlayView.RaycastTargetMode.OnZero;
        
        void OnClick()
        {
            if (completed)
                return;
            
            if (!animationsCompleted)
            {
                if (!openStarted)
                {
                    Animator.SetBool(ShowID, true);
                    openStarted = true;
                }
                else if (!chestOpened)
                    Animator.SetFloat(SpeedID, 2.5f);
                else
                    actionQueue.Reset();
            }
            else
            {
                Hide();
                completed = true;
                onSuccess?.Invoke();
            }
        }
        
        Overlay = blurOverlay;
        Overlay.OnClick += OnClick;
        
        OkButton.Text = "Ok";
        OkButton.OnClick += OnClick;
        OkButton.HideComponent(0);

        actionQueue = gameObject.AddComponent<GameActionQueue>();

        actionQueue.Add(new WaitUntilAction(() => chestOpened));

        RewardContainers rewardContainers =
            rewardContainersList.First(containers => containers.Count == rewards.Length);

        float duration = (float) 1 / rewards.Length;
        
        for (int i = 0; i < rewards.Length; i++)
            actionQueue.Add(new RewardAnimationAction(duration, rewardContainers.containers[i], chestSource, rewards[i]));

        actionQueue.Add(new CallbackAction(() => OkButton.ShowComponent()));
        actionQueue.Add(new CallbackAction(() => animationsCompleted = true));
        
        RectTransform.localScale = Vector2.zero;
        Show();
    }

    [UsedImplicitly]
    void ShowRewards()
    {
        chestOpened = true;
    }

    #region actions
    
    class RewardAnimationAction : GameAction
    {
        readonly Reward Reward;
        readonly RectTransform Container;
        readonly RectTransform Source;
        readonly float Duration;

        IEnumerator delayRoutine;
        RewardShine rewardShine;
        bool aborted;
        
        public RewardAnimationAction(float duration,  RectTransform container, RectTransform source, Reward reward)
        {
            Reward = reward;
            Container = container;
            Source = source;
            Duration = duration;
        }
        
        public override void Start()
        {
            rewardShine = RewardShine.Create(Container, Reward.Rarity);

            Reward.CreateView(rewardShine.RectTransform);
            
            rewardShine.RectTransform.localScale = Vector3.zero;
            rewardShine.RectTransform.position = Source.position;

            int count = 2;

            void Finished()
            {
                count--;
                if (count > 0) return;
                Pop();
            }

            rewardShine.RectTransform.DOScale(Vector3.one, Duration).OnComplete(Finished);
            rewardShine.RectTransform.DOMove(Container.position, Duration).OnComplete(Finished);
        }

        public override void Update() { }
        
        public override void Abort()
        {
            rewardShine.RectTransform.DOKill();
            rewardShine.RectTransform.position = Container.position;
            rewardShine.RectTransform.localScale = Vector3.one;
        }

        public override void Dispose()
        {
            rewardShine.RectTransform.DOKill();
            rewardShine.RectTransform.position = Container.position;
            rewardShine.RectTransform.localScale = Vector3.one;
        }

    } 
    #endregion
}
