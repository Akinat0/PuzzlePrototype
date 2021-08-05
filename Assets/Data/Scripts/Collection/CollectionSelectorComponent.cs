using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abu.Tools;
using Abu.Tools.UI;
using Data.Scripts.Tools.Input;
using DG.Tweening;
using Puzzle.Analytics;
using ScreensScripts;
using UnityEngine;

public class CollectionSelectorComponent : SelectorComponent<CollectionItem>
{
    #region serialized
    
    [SerializeField] TextButtonComponent InteractBtn;
    [SerializeField] ButtonComponent HomeBtn;
    [SerializeField] Transform ItemsContainer;
    [SerializeField] RectTransform Content;
    [SerializeField] CollectionColorSelector ColorSelector;

    #endregion
    
    #region propeties
    
    int NextItem
    {
        get
        {
            int diff = Mathf.CeilToInt(Mathf.Abs(Offset - Index));
            int nextLevel = Offset > Index ? Index + diff : Index - diff;
            return nextLevel;
        }
    }

    string SelectText => "Select";
    string SetAsDefaultText => "SetAsDefault";
    
    #endregion
    
    #region attributes

    readonly Dictionary<int, PlayerView> itemContainers = new Dictionary<int, PlayerView>();
    
    IEnumerator afterTouchRoutine;
    IEnumerator moveToIndexRoutine;

    #endregion
    
    #region private
    
    void Start()
    {
        ItemsContainer.SetY(ScreenScaler.CameraSize.y);
        Selection = Account.CollectionItems;

        LauncherUI.Instance.LauncherTextGroup.Add(new TextObject(InteractBtn.TextField.TextMesh,
            possibleContent: new[] { SetAsDefaultText }));
        
        HideCollection();
    }

    protected override void MoveLeft()
    {
        if (!HasItem(Index - 1) || !LeftBtn.Interactable || !MobileInput.Condition) 
            return;

        IsFocused = false;
        
        float phase = Mathf.Abs(Offset - Index);
            
        if(moveToIndexRoutine != null)
            StopCoroutine(moveToIndexRoutine);
        StartCoroutine(moveToIndexRoutine =
            MoveToIndexRoutine(Index - 1, (1 - phase) * LevelSelectorComponent.UiAnimationDuration / 2, () =>
            {
                moveToIndexRoutine = null;
                IsFocused = true;
            }));
    }

    protected override void MoveRight()
    {
        if (!HasItem(Index + 1) || !RightBtn.Interactable || !MobileInput.Condition) 
            return;

        IsFocused = false;
        
        float phase = Mathf.Abs(Offset - Index);
            
        if(moveToIndexRoutine != null)
            StopCoroutine(moveToIndexRoutine);
        StartCoroutine(moveToIndexRoutine =
            MoveToIndexRoutine(Index + 1, (1 - phase) * LevelSelectorComponent.UiAnimationDuration / 2, () =>
            {
                moveToIndexRoutine = null;
                IsFocused = true;
            }));
    }

    void CreateItem(int index)
    {
        if (index < 0 || index >= Length)
            return;

        if (itemContainers.ContainsKey(index))
            return;

        Transform collectionItem = PlayerView.Create(ItemsContainer, Selection[index].ID, LauncherUI.Instance.LevelConfig.PuzzleSides).transform;
        
        collectionItem.localPosition = index * ScreenScaler.CameraSize * Vector2.right;
        itemContainers[index] = collectionItem.GetComponent<PlayerView>();
    }

    void SetupColors(LevelColorScheme colorScheme)
    {
        LeftBtn.Color = colorScheme.ArrowColor;
        RightBtn.Color = colorScheme.ArrowColor;
        
        colorScheme.SetButtonColor(InteractBtn);
        colorScheme.SetButtonColor(HomeBtn);
    }
    
    void ShowCollection(int? ItemID)
    {
        SetupColors(LauncherUI.Instance.LevelConfig.ColorScheme);

        UpdateInteractButtonText();

        int index = Account.CollectionDefaultItemId; 
        
        if (ItemID != null)
            index = Array.FindIndex(Selection, (entry) => entry.ID == ItemID);

        Index = index; 
        
        Content.gameObject.SetActive(true);

        CreateItem(Index);
        CreateItem(Index - 1);
        CreateItem(Index + 1);
        
        Content.DOAnchorPos(Vector2.zero, LevelSelectorComponent.UiAnimationDuration)
            .SetDelay(LevelSelectorComponent.UiAnimationDuration / 2);

        Vector3 targetContainerPosition = ItemsContainer.position;
        targetContainerPosition.y = 0;
        
        ItemsContainer.DOMove(targetContainerPosition, LevelSelectorComponent.UiAnimationDuration)
            .SetDelay(LevelSelectorComponent.UiAnimationDuration / 2);

        this.Invoke(()=> IsFocused = true, LevelSelectorComponent.UiAnimationDuration);
    }
    
    void HideCollection(bool animated = false)
    {
        ProcessIndex();
        
        if (animated)
            Content.DOAnchorPos(Vector3.up * ScreenScaler.ScreenSize.y, LevelSelectorComponent.UiAnimationDuration);
        else
            Content.anchoredPosition = Vector3.up * ScreenScaler.ScreenSize.y;

        Vector3 targetContainerPosition = ItemsContainer.position;
        targetContainerPosition.y = ScreenScaler.CameraSize.y;

        ItemsContainer.DOMove(targetContainerPosition, LevelSelectorComponent.UiAnimationDuration).onComplete += CleanContainers;

        IsFocused = false;
    }

    void CleanContainers()
    {
        foreach (int key in itemContainers.Keys)
            DestroyImmediate(itemContainers[key].gameObject);
        
        itemContainers.Clear();
    }

    void UpdateInteractButtonText()
    {
        LauncherUI.Instance.LevelConfig.ColorScheme.SetButtonColor(InteractBtn);
        
        if (Current.Unlocked)
        {
            InteractBtn.Text = LauncherUI.Instance.LevelConfig.CollectionEnabled ? SelectText : SetAsDefaultText;
        }
        else
        {
            Wallet shards = Account.GetShards(Current.Rarity);
            InteractBtn.Text = $"{shards.Amount}/{Current.Cost}{EmojiHelper.GetShardEmojiText(Current.Rarity)}";
            
            if (!shards.Has(Current.Cost))
                InteractBtn.TextField.Color = new Color(1f, 0.49f, 0.58f);
        }
    }
    
    void OnChoose()
    {
        if (!IsFocused || !itemContainers.ContainsKey(Index))
            return;
        
        ProcessIndex();

        if (!Current.Unlocked)
        {
            Tier puzzleTier = Account.Tiers.Where(tier => tier.Type == Tier.TierType.Puzzle).FirstOrDefault(tier => tier.Reward is PuzzleReward puzzleReward && puzzleReward.PuzzleID == Current.ID);

            if(puzzleTier == null)
                return;

            TierWindow.Create(puzzleTier, UpdateInteractButtonText);
            return;
        }

        PlayerView newPlayerView = LauncherUI.Instance.LevelConfig.CollectionEnabled ? itemContainers[Index] : null;
        
        if(LauncherUI.Instance.LevelConfig.CollectionEnabled)
            itemContainers.Remove(Index);

        string previousPuzzleName = Account.CollectionDefaultItem.Name;
        
        Account.CollectionDefaultItemId = Index;
        
        LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs(newPlayerView));
        
        SendPuzzleChangedAnalyticsEvent(previousPuzzleName);
    }

    void OnBack()
    {
        if (!IsFocused)
            return;

        LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs());
    }

    bool HasItem(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < Length;
    }
    
    #endregion
    
    #region Offset

    protected override void ProcessOffset()
    {
        ItemsContainer.SetX(- Offset * ScreenScaler.CameraSize.x);

        ProcessItems();
        ProcessSideButtons();
    }

    void ProcessItems()
    {
        int nextItem = NextItem;
        
        if(!HasItem(nextItem))
            return;
        
        CreateItem(nextItem);
    }
    
    void ProcessSideButtons()
    {
        int nextItem = NextItem;

        if (!HasItem(nextItem))
            return;
        
        float phase = Mathf.Abs(Offset - Index);

        int direction = Index > nextItem ? 1 : -1;

        Color startLeftBtnColor = HasItem(Index - 1) ? Color.white : Color.clear;
        Color startRightBtnColor = HasItem(Index + 1) ? Color.white : Color.clear;
        
        Color targetLeftBtnColor = HasItem(Index - direction - 1) ? Color.white : Color.clear;
        Color targetRightBtnColor = HasItem(Index - direction + 1) ? Color.white : Color.clear;

        LeftBtn.Color = Color.Lerp(startLeftBtnColor, targetLeftBtnColor, phase * phase);
        RightBtn.Color = Color.Lerp(startRightBtnColor, targetRightBtnColor, phase * phase);
        
    }
    #endregion
    
    #region Index

    protected override void ProcessIndex()
    {
        if(afterTouchRoutine != null)
            StopCoroutine(afterTouchRoutine);

        if(moveToIndexRoutine != null)
            StopCoroutine(moveToIndexRoutine);
        
        CreateItem(Index - 1);
        CreateItem(Index);
        CreateItem(Index + 1);
        
        Offset = Index;
        
        ItemsContainer.SetX(- Index * ScreenScaler.CameraSize.x);
        
        ColorSelector.ProcessIndex(Index, Selection);
        
        ProcessSideButtonsByIndex();
        ProcessInteractButtonByIndex();
    }
    
    void ProcessSideButtonsByIndex()
    {
        RightBtn.Interactable = Index + 1 < Length;
        LeftBtn.Interactable = Index > 0;
        
        RightBtn.Color = Index + 1 < Length ? Color.white : Color.clear;
        LeftBtn.Color = Index > 0 ? Color.white : Color.clear;
    }

    void ProcessInteractButtonByIndex()
    {
        UpdateInteractButtonText();
    }
    
    #endregion
    
    #region other

    void SendPuzzleChangedAnalyticsEvent(string previousPuzzleName)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>()
        {
            {"puzzle_name", Account.CollectionDefaultItem.Name},
            {"puzzle_color", Account.CollectionDefaultItem.ActiveColorIndex},
            {"previous_puzzle", previousPuzzleName ?? string.Empty},
            {"level_name", LauncherUI.Instance.LevelConfig.Name},
            {"stars", Account.Stars.Amount}
        };

        new SimpleAnalyticsEvent("puzzle_changed", eventData).Send();
    }
    
    #endregion
    
    #region event handlers

    protected override void OnEnable()
    {
        base.OnEnable();
        LauncherUI.ShowCollectionEvent += ShowCollectionEvent_Handler;
        LauncherUI.CloseCollectionEvent += CloseCollectionEvent_Handler;
        InteractBtn.OnClick += OnChoose;
        HomeBtn.OnClick += OnBack;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        LauncherUI.ShowCollectionEvent -= ShowCollectionEvent_Handler;
        LauncherUI.CloseCollectionEvent -= CloseCollectionEvent_Handler;
        InteractBtn.OnClick -= OnChoose;
        HomeBtn.OnClick -= OnBack;
    }
    
    protected override void OnTouchDown_Handler(Vector2 position)
    {
        if (!IsFocused)
            return;
        
        if(afterTouchRoutine != null)
            StopCoroutine(afterTouchRoutine);

        if(moveToIndexRoutine != null)
            StopCoroutine(moveToIndexRoutine);
    }

    protected override void OnTouchMove_Handler(Vector2 delta)
    {
        if (!IsFocused)
            return;
        
        float offsetDelta = - delta.x / ScreenScaler.ScreenSize.x * TouchSensitivity; 

        bool shouldMove = Offset + offsetDelta >= 0 && Offset + offsetDelta < Length - 1;
        
        if(shouldMove)
            Offset += offsetDelta;
        
    }

    protected override void OnTouchCancel_Handler(Vector2 position)
    {
        if (!IsFocused)
            return;
        
        StartCoroutine(afterTouchRoutine = TimedAfterTouchRoutine(0.3f));
    }
    
    void ShowCollectionEvent_Handler(ShowCollectionEventArgs _Args)
    {
        ShowCollection(_Args.ItemID);
    }

    void CloseCollectionEvent_Handler(CloseCollectionEventArgs _)
    {
        HideCollection(true);
    }
    
    #endregion
}
