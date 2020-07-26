using System;
using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using Abu.Tools.UI;
using Data.Scripts.Tools.Input;
using ScreensScripts;
using UnityEngine;
using DG.Tweening;
using Debug = UnityEngine.Debug;

public class LevelSelectorComponent : SelectorComponent<LevelConfig>
{
    #region serialized
    
    [SerializeField] Transform LevelsContainer;
    [SerializeField] TextButtonComponent InteractBtn;
    [SerializeField] TextButtonComponent CollectionBtn;

    #endregion

    #region properties

    protected override int Index
    {
        get => base.Index;
        set
        {
            Account.DefaultLevelId = value;
            base.Index = value;
        }
    }
    
    int NextLevel
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

    readonly Dictionary<int, LevelRootView> levelContainers = new Dictionary<int, LevelRootView>();

    IEnumerator afterTouchRoutine;
    IEnumerator moveToIndexRoutine;

    PlayerView playerView;

    #endregion

    #region constants
    
    public const float MainButtonsOffset = 500;
    public const float UiAnimationDuration = 0.5f;

    static readonly Vector2 EnabledCollectionMaxAnchor = new Vector2(0.48f, 0.3f);
    static readonly Vector2 DisabledCollectionMaxAnchor = new Vector2(0.14f, 0.3f);
    
    static readonly Vector2 EnabledPlayMaxAnchor = new Vector2(0.86f,0.3f);
    static readonly Vector2 DisabledPlayMaxAnchor = new Vector2(0.75f,0.3f);
    
    static readonly Vector2 EnabledPlayMinAnchor = new Vector2(0.515f,0.24f);
    static readonly Vector2 DisabledPlayMinAnchor = new Vector2(0.25f,0.24f);

    #endregion

    #region private
    
    void Start()
    {
        Selection = Account.LevelConfigs;
        Index = Mathf.Clamp(Account.DefaultLevelId, 0, Length - 1);
        Offset = Index;
        ShowUI();
        ProcessIndex();
    }
    
    protected override void MoveLeft()
    {
        if (!HasLevel(Index - 1) || !LeftBtn.Interactable || !MobileInput.Condition) 
            return;
        
        float phase = Mathf.Abs(Offset - Index);
            
        if(moveToIndexRoutine != null)
            StopCoroutine(moveToIndexRoutine);
        StartCoroutine(moveToIndexRoutine =
            MoveToIndexRoutine(Index - 1, (1 - phase) * UiAnimationDuration / 2, () => moveToIndexRoutine = null));
    }

    protected override void MoveRight()
    {
        if (!HasLevel(Index + 1) || !RightBtn.Interactable || !MobileInput.Condition) 
            return;
        
        float phase = Mathf.Abs(Offset - Index);
            
        if(moveToIndexRoutine != null)
            StopCoroutine(moveToIndexRoutine);
        StartCoroutine(moveToIndexRoutine =
            MoveToIndexRoutine(Index + 1, (1 - phase) * UiAnimationDuration / 2, () => moveToIndexRoutine = null));
    }
    
    void HideUI()
    {
        RightBtn.RectTransform.DOAnchorPos(new Vector2(210, 0), UiAnimationDuration);
        LeftBtn.RectTransform.DOAnchorPos(new Vector2(-210, 0), UiAnimationDuration);
        
        Tweener interactBtnTweener = InteractBtn.RectTransform.DOAnchorPos(new Vector2(0, InteractBtn.RectTransform.rect.y - MainButtonsOffset), UiAnimationDuration);
        interactBtnTweener.onPlay = () => InteractBtn.Interactable = false; 
        interactBtnTweener.onComplete = () => InteractBtn.SetActive(false);
        
        HideCollectionButton(UiAnimationDuration);

        IsFocused = false;
    }

    void ShowUI()
    {
        RightBtn.SetActive(true);
        LeftBtn.SetActive(true);
        RightBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        LeftBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        
        InteractBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f)
            .onPlay = () =>
        {
            InteractBtn.Interactable = true;
            InteractBtn.SetActive(true);
        };

        ShowCollectionButton(UiAnimationDuration, 0.25f);

        CreateLevel(Index);
        CreateLevel(Index - 1);
        CreateLevel(Index + 1);

        this.Invoke(() => IsFocused = true, UiAnimationDuration);
    }
    
    void BringBackUI(PlayerView newPlayerView)
    {
        
        RightBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        LeftBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        
        Tweener interactBtnTweener = InteractBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        interactBtnTweener.onPlay = () =>
        {
            InteractBtn.Interactable = true;
            InteractBtn.SetActive(true);
        };

        ShowCollectionButton(UiAnimationDuration, 0.25f);
        
        if (newPlayerView != null)
        {    
            //Remove the old player view with new one
            DestroyImmediate(playerView.gameObject);
            playerView = newPlayerView;
            levelContainers[Index].PlayerView = playerView;
            
            foreach (int levelIndex in levelContainers.Keys)
            {
                if (levelIndex == Index)
                    continue;
            
                SetLevelDefaultPlayerView(levelIndex);
            }
            
        }
        else
        {
            //Bring back old player view
            playerView.transform.DOMove(Vector3.zero, UiAnimationDuration).SetDelay(0.25f);
        }

        this.Invoke(() => IsFocused = true, UiAnimationDuration);
    }

    void HideActivePlayer()
    {
        playerView.transform.DOMove(Vector3.down * ScreenScaler.CameraSize.y, UiAnimationDuration);
    }

    void ShowCollectionButton(float _Duration = 0, float _Delay = 0)
    {
        CollectionBtn.SetActive(true);
        CollectionBtn.Interactable = true;
        
        CollectionBtn.RectTransform.DOAnchorPos(Vector2.zero, _Duration).SetDelay(_Delay);
    }
    
    void HideCollectionButton(float _Duration)
    {
        if (_Duration < Mathf.Epsilon)
        {
            CollectionBtn.RectTransform.anchoredPosition = new Vector2(0, CollectionBtn.RectTransform.rect.y - MainButtonsOffset);
        }
        else
        {
            CollectionBtn.RectTransform.DOAnchorPos(new Vector2(0, CollectionBtn.RectTransform.rect.y - MainButtonsOffset), _Duration)
                .OnStart(() => { CollectionBtn.Interactable = false; })
                .SetUpdate(true)
                .onComplete = () => CollectionBtn.SetActive(false);
        }
    }

    void CleanContainers()
    {
        int count = Length;
        
        for (int i = 0; i < count; i++)
        {
            if (i != Index && levelContainers.ContainsKey(i))
            {
                Destroy(levelContainers[i].gameObject);
                levelContainers.Remove(i);
            }
        }
    }

    void ShowCollection()
    {
        if (!Current.CollectionEnabled)
            return;
        
        LauncherUI.Instance.InvokeShowCollection(new ShowCollectionEventArgs(Current.ColorScheme));
        HideUI();
        HideActivePlayer();
    }

    void CreateLevel(int index)
    {
        if (index < 0 || index >= Length)
            return;

        if (levelContainers.ContainsKey(index))
        {
            levelContainers[index].SetActiveLevelRoot(true);
            return;
        }

        Transform level = Instantiate(Selection[index].LevelRootPrefab, LevelsContainer).transform;
        level.localPosition = index * ScreenScaler.CameraSize * Vector2.right;
        LevelRootView levelRootView = level.GetComponent<LevelRootView>();
        levelContainers[index] = levelRootView;

        SetLevelDefaultPlayerView(index);
    }

    void SetLevelDefaultPlayerView(int index)
    {
        if (index < 0 || index >= Length || !Selection[index].CollectionEnabled)
            return;

        LevelRootView levelRootView = levelContainers[index];
        
        GameObject newPlayerViewPrefab = Account.CollectionDefaultItem.GetPuzzleVariant(Selection[index].PuzzleSides);

        if (!newPlayerViewPrefab)
            return;
            
        DestroyImmediate(levelRootView.PlayerView.gameObject);

        PlayerView newPlayerView = Instantiate(newPlayerViewPrefab).GetComponent<PlayerView>();

        levelRootView.PlayerView = newPlayerView;
    }
        
    void OnInteract()
    {
        LauncherUI.Instance.InvokePlayLauncher(new PlayLauncherEventArgs(Current));
        
        CleanContainers();
        
        IsFocused = false;
    }

    void OnCollection()
    {
        ShowCollection();
    }

    bool HasLevel(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < Length;
    }
    
    #endregion
    
    #region Offset
    
    protected override void ProcessOffset()
    {
        LevelsContainer.position = - Offset * ScreenScaler.CameraSize.x * Vector3.right;

        ProcessLevels();
        ProcessColors();
        ProcessButtons();
        ProcessSideButtons();
        
        int closestIndex = Mathf.RoundToInt(Offset);
        if (Mathf.Abs(closestIndex - Offset) <= 0.005f && closestIndex != Index) // Like epsilon
            Index = closestIndex;
    }

    void ProcessButtons()
    {
        int nextLevel = NextLevel;

        if (!HasLevel(nextLevel))
            return;
        
        float phase = Mathf.Abs(Offset - Index) / 1;
        
        Vector2 startCollectionBtnMaxAnchor = 
            Current.CollectionEnabled ? EnabledCollectionMaxAnchor : DisabledCollectionMaxAnchor;
        Vector2 targetCollectionBtnMaxAnchor = 
            Selection[nextLevel].CollectionEnabled ? EnabledCollectionMaxAnchor : DisabledCollectionMaxAnchor;

        CollectionBtn.RectTransform.anchorMax =
            Vector2.Lerp(startCollectionBtnMaxAnchor, targetCollectionBtnMaxAnchor, phase * phase);

        Vector2 startPlayBtnMinAnchor = 
            Current.CollectionEnabled ? EnabledPlayMinAnchor : DisabledPlayMinAnchor;
        Vector2 targetPlayBtnMinAnchor = 
            Selection[nextLevel].CollectionEnabled ? EnabledPlayMinAnchor : DisabledPlayMinAnchor;
        
        Vector2 startPlayBtnMaxAnchor = 
            Current.CollectionEnabled ? EnabledPlayMaxAnchor : DisabledPlayMaxAnchor;
        Vector2 targetPlayBtnMaxAnchor = 
            Selection[nextLevel].CollectionEnabled ? EnabledPlayMaxAnchor : DisabledPlayMaxAnchor;
        
        InteractBtn.RectTransform.anchorMin =
            Vector2.Lerp(startPlayBtnMinAnchor, targetPlayBtnMinAnchor, phase * phase);
        InteractBtn.RectTransform.anchorMax =
            Vector2.Lerp(startPlayBtnMaxAnchor, targetPlayBtnMaxAnchor, phase * phase);
        
        float startCollectionTextAlpha = Current.CollectionEnabled ? 1 : 0;
        float targetCollectionTextAlpha = Selection[nextLevel].CollectionEnabled ? 1 : 0;

        CollectionBtn.TextField.SetAlpha(Mathf.Lerp(startCollectionTextAlpha, targetCollectionTextAlpha, phase * phase));
    }
    
    void ProcessSideButtons()
    {
        int nextLevel = NextLevel;

        if (!HasLevel(nextLevel))
            return;
        
        float phase = Mathf.Abs(Offset - Index) / 1;

        int direction = Index > nextLevel ? 1 : -1;

        Color startLeftBtnColor = HasLevel(Index - 1) ? Color.white : Color.clear;
        Color startRightBtnColor = HasLevel(Index + 1) ? Color.white : Color.clear;
        
        Color targetLeftBtnColor = HasLevel(Index - direction - 1) ? Color.white : Color.clear;
        Color targetRightBtnColor = HasLevel(Index - direction + 1) ? Color.white : Color.clear;

        LeftBtn.Color = Color.Lerp(startLeftBtnColor, targetLeftBtnColor, phase * phase);
        RightBtn.Color = Color.Lerp(startRightBtnColor, targetRightBtnColor, phase * phase);
        
    }
    
    void ProcessLevels()
    {
        int nextLevel = NextLevel;
        
        if(!HasLevel(nextLevel))
            return;
        
        CreateLevel(nextLevel);
    }
    
    void ProcessColors()
    {
        int nextLevel = NextLevel;

        if (!HasLevel(nextLevel))
            return;
        
        float phase = Mathf.Abs(Offset - Index) / 1; 

        Color buttonsColor = Color.Lerp(Current.ColorScheme.ButtonColor,
            Selection[nextLevel].ColorScheme.ButtonColor,
            phase
        );
        
        Color walletTextColor = Color.Lerp(Current.ColorScheme.TextColorLauncher,
            Selection[nextLevel].ColorScheme.TextColorLauncher,
            phase
        );
        
        Color buttonsTextColor = Color.Lerp(Current.ColorScheme.TextColor,
            Selection[nextLevel].ColorScheme.TextColor,
            phase
        );

        InteractBtn.Color = buttonsColor;
        CollectionBtn.Color = buttonsColor;
        LauncherUI.Instance.UiManager.Wallet.Text.color = walletTextColor;
        InteractBtn.TextField.color = buttonsTextColor;
        CollectionBtn.TextField.color = buttonsTextColor;

    }
    
    #endregion
    
    #region Index

    protected override void ProcessIndex()
    {
        if(afterTouchRoutine != null)
            StopCoroutine(afterTouchRoutine);

        if(moveToIndexRoutine != null)
            StopCoroutine(moveToIndexRoutine);
        
        CreateLevel(Index - 1);
        CreateLevel(Index);
        CreateLevel(Index + 1);

        if(levelContainers.ContainsKey(Index))
            playerView = levelContainers[Index].PlayerView;
        else
            Debug.LogError("Key doesn't exists " + Index);
        
        LauncherUI.Instance.InvokeLevelChanged(new LevelChangedEventArgs(playerView, Current));
        
        Offset = Index;
        
        LevelsContainer.position = - Index * ScreenScaler.CameraSize.x * Vector3.right;
        
        ProcessLevelsByIndex();
        ProcessNameByIndex();
        ProcessSideButtonsByIndex();
        ProcessColorsByIndex();
        ProcessButtonsByIndex();
    }

    void ProcessButtonsByIndex()
    {
        CollectionBtn.RectTransform.anchorMax = Current.CollectionEnabled ? EnabledCollectionMaxAnchor : DisabledCollectionMaxAnchor;
        
        InteractBtn.RectTransform.anchorMax = Current.CollectionEnabled ? EnabledPlayMaxAnchor : DisabledPlayMaxAnchor;
        InteractBtn.RectTransform.anchorMin = Current.CollectionEnabled ? EnabledPlayMinAnchor : DisabledPlayMinAnchor;
        
        CollectionBtn.TextField.SetAlpha(Current.CollectionEnabled ? 1 : 0);
    }
    
    void ProcessLevelsByIndex()
    {
        CreateLevel(Index - 1);
        CreateLevel(Index + 1);

        foreach (KeyValuePair<int, LevelRootView> levelContainer in levelContainers)
        {
            if(levelContainer.Key != Index - 1 && levelContainer.Key != Index && levelContainer.Key != Index + 1)
                levelContainer.Value.SetActiveLevelRoot(false);
        }
    }
    void ProcessNameByIndex()
    {
        InteractBtn.Text = Current.Name;
    }

    void ProcessSideButtonsByIndex()
    {
        RightBtn.Color = Index + 1 < Length ? Color.white : Color.clear;
        LeftBtn.Color = Index > 0 ? Color.white : Color.clear;
        
        RightBtn.Interactable = Index + 1 < Length;
        LeftBtn.Interactable = Index > 0;
    }
    
    void ProcessColorsByIndex()
    {
        CollectionBtn.Color = Current.ColorScheme.ButtonColor;
        InteractBtn.Color = Current.ColorScheme.ButtonColor;
        LauncherUI.Instance.UiManager.Wallet.Text.color = Current.ColorScheme.TextColorLauncher;
        InteractBtn.TextField.color = Current.ColorScheme.TextColor;
        CollectionBtn.TextField.color = Current.ColorScheme.TextColor;
    }
    
    #endregion

    #region event handlers
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
        LauncherUI.GameSceneUnloadedEvent += GameSceneUnloadedEvent_Handler;
        LauncherUI.CloseCollectionEvent += CloseCollectionEvent_Handler;
        
        InteractBtn.OnClick += OnInteract;
        CollectionBtn.OnClick += OnCollection;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
        LauncherUI.GameSceneUnloadedEvent -= GameSceneUnloadedEvent_Handler;
        LauncherUI.CloseCollectionEvent -= CloseCollectionEvent_Handler;
        
        InteractBtn.OnClick -= OnInteract;
        CollectionBtn.OnClick -= OnCollection;
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
    
    void PlayLauncherEvent_Handler(PlayLauncherEventArgs _Args)
    {
        HideUI();
    }

    void GameSceneUnloadedEvent_Handler()
    {
        ShowUI();   
    }

    //The handler handles two behaviours: if we chose new player or if we not
    void CloseCollectionEvent_Handler(CloseCollectionEventArgs _Args)
    {
        BringBackUI(_Args.PlayerView);
    }
    
    #endregion
    
}
