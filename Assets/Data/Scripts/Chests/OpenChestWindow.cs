
using System;
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
        
        void OnSuccess()
        {
            if (!animationsCompleted)
            {
                if (!openStarted)
                    Animator.SetBool(ShowID, true);
                else if (!chestOpened)
                    Animator.SetFloat(SpeedID, 2.5f);
                else
                    actionQueue.Reset();
            }
            else
            {
                Hide();
                onSuccess?.Invoke();
            }
        }
        
        Overlay = blurOverlay;
        Overlay.OnClick += OnSuccess;
        
        OkButton.Text = "Ok";
        OkButton.OnClick += OnSuccess;
        OkButton.HideComponent(0);

        actionQueue = gameObject.AddComponent<GameActionQueue>();

        actionQueue.Add(new WaitUntilAction(() => chestOpened));

        RewardContainers rewardContainers =
            rewardContainersList.First(containers => containers.Count == rewards.Length);

        for (int i = 0; i < rewards.Length; i++)
        {
            actionQueue.Add(new RewardAnimationAction(rewardContainers.containers[i], chestSource, rewards[i]));
            actionQueue.Add(new WaitAction(i * 0.1f));
        }
        
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

        RectTransform rewardTransform;
        
        public RewardAnimationAction(RectTransform container, RectTransform source, Reward reward)
        {
            Reward = reward;
            Container = container;
            Source = source;
        }
        
        public override void Start()
        {
            rewardTransform = Reward.CreateView(Container).RectTransform;
            
            rewardTransform.localScale = Vector2.zero;
            rewardTransform.position = Source.position;

            int count = 2;

            void Finished()
            {
                count--;
                if(count > 0) return;
                Pop();
            }
            
            rewardTransform.DOScale(Vector3.one, 1).OnComplete(Finished);
            rewardTransform.DOMove(Container.position, 1).OnComplete(Finished);
        }

        public override void Update() { }


        public override void Abort()
        {
            rewardTransform.DOComplete();
        }

        public override void Dispose()
        {
            rewardTransform.DOComplete();
        }
    } 
    #endregion
}
