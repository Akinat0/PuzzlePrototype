using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abu.Tools;
using Abu.Tools.UI;
using Data.Scripts.Tools.Input;
using DG.Tweening;
using ScreensScripts;
using UnityEngine;

public class CollectionSelectorComponent : SelectorComponent<CollectionItem>
{
    #region serialized
    
    [SerializeField] TextButtonComponent InteractBtn;
    [SerializeField] ButtonComponent HomeBtn;
    [SerializeField] Transform ItemsContainer;
    [SerializeField] RectTransform Content;

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
        HideCollection();
    }

    protected override void MoveLeft()
    {
        if (!HasItem(Index - 1) || !LeftBtn.Interactable || !MobileInput.Condition) 
            return;
        
        float phase = Mathf.Abs(Offset - Index);
            
        if(moveToIndexRoutine != null)
            StopCoroutine(moveToIndexRoutine);
        StartCoroutine(moveToIndexRoutine =
            MoveToIndexRoutine(Index - 1, (1 - phase) * LevelSelectorComponent.UiAnimationDuration / 2, () => moveToIndexRoutine = null));
    }

    protected override void MoveRight()
    {
        if (!HasItem(Index + 1) || !RightBtn.Interactable || !MobileInput.Condition) 
            return;
        
        float phase = Mathf.Abs(Offset - Index);
            
        if(moveToIndexRoutine != null)
            StopCoroutine(moveToIndexRoutine);
        StartCoroutine(moveToIndexRoutine =
            MoveToIndexRoutine(Index + 1, (1 - phase) * LevelSelectorComponent.UiAnimationDuration / 2, () => moveToIndexRoutine = null));
    }

    void CreateItem(int index)
    {
        if (index < 0 || index >= Length)
            return;

        if (itemContainers.ContainsKey(index))
            return;

        Transform collectionItem =
            Instantiate(Selection[index].GetPuzzleVariant(LauncherUI.Instance.LevelConfig.PuzzleSides), ItemsContainer)
                .transform;
        collectionItem.localPosition = index * ScreenScaler.CameraSize * Vector2.right;
        itemContainers[index] = collectionItem.GetComponent<PlayerView>();
    }

    void SetupColors(LevelColorScheme colorScheme)
    {
        LeftBtn.Color = colorScheme.ArrowColor;
        RightBtn.Color = colorScheme.ArrowColor;
        InteractBtn.Color = colorScheme.ButtonColor;
        HomeBtn.Color = colorScheme.ButtonColor;
        InteractBtn.TextField.color = colorScheme.TextColor;
    }
    
    void ShowCollection(int? ItemID)
    {
        SetupColors(LauncherUI.Instance.LevelConfig.ColorScheme);

        InteractBtn.Text = LauncherUI.Instance.LevelConfig.CollectionEnabled ? "Select" : "Set As Default";
        
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
    
    void HideCollection(float _Duration = 0)
    {
        if (Math.Abs(_Duration) > Mathf.Epsilon)
            Content.DOAnchorPos(Vector3.up * Screen.height, LevelSelectorComponent.UiAnimationDuration);
        else
            Content.position += Vector3.up * Screen.height;

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
    
    void OnChoose()
    {
        if (!IsFocused || !itemContainers.ContainsKey(Index))
            return;

        PlayerView newPlayerView = LauncherUI.Instance.LevelConfig.CollectionEnabled ? itemContainers[Index] : null;
        
        if(LauncherUI.Instance.LevelConfig.CollectionEnabled)
            itemContainers.Remove(Index);

        Account.CollectionDefaultItemId = Index;
        
        LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs(newPlayerView));

        HideCollection();
    }

    void OnBack()
    {
        if (!IsFocused)
            return;
        
        Back();
    }

    void Back()
    {
        HideCollection(LevelSelectorComponent.UiAnimationDuration);
        LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs(null));
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
        
        float phase = Mathf.Abs(Offset - Index) / 1;

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
        
        ProcessSideButtonsByIndex();
    }
    
    void ProcessSideButtonsByIndex()
    {
        RightBtn.Interactable = Index + 1 < Length;
        LeftBtn.Interactable = Index > 0;
        
        RightBtn.Color = Index + 1 < Length ? Color.white : Color.clear;
        LeftBtn.Color = Index > 0 ? Color.white : Color.clear;
    }
    #endregion
    
    #region event handlers

    protected override void OnEnable()
    {
        base.OnEnable();
        LauncherUI.ShowCollectionEvent += ShowCollectionEvent_Handler;
        InteractBtn.OnClick += OnChoose;
        HomeBtn.OnClick += OnBack;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        LauncherUI.ShowCollectionEvent -= ShowCollectionEvent_Handler;
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
    
    #endregion

    
}
