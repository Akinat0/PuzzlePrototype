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
    
    [SerializeField] Transform LevelContainer;
    [SerializeField] TextButtonComponent InteractBtn;
    [SerializeField] TextButtonComponent CollectionBtn;

    #endregion
    
    PlayerView m_PlayerView;
    
    protected override int Index
    {
        get => base.Index;
        set
        {
            base.Index = value;
            Account.DefaultLevelId = value;

            ProcessIndex();
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

    readonly Dictionary<int, LevelRootView> levelContainers = new Dictionary<int, LevelRootView>();

    IEnumerator afterTouchRoutine;
    IEnumerator moveToIndexRoutine;

    #region constants
    
    public const float MainButtonsOffset = 500; 
    public const float PlayerAnimationDuration = 0.5f; 
    public const float UiAnimationDuration = 0.5f;

    public const float TouchSensitivity = 0.8f;
    
    #endregion
    
    public void Start()
    {
        Selection = Account.LevelConfigs;
        Index = Mathf.Clamp(Account.DefaultLevelId, 0, Length - 1);
        Offset = Index;
        ShowUI();
        ProcessIndex();
    }
    
    protected override void MoveLeft()
    {
        if (HasLevel(Index - 1) && LeftBtn.Interactable && MobileInput.Condition)
        {
            float phase = Mathf.Abs(Offset - Index) / 1;
            
            if(moveToIndexRoutine != null)
                StopCoroutine(moveToIndexRoutine);
            StartCoroutine(moveToIndexRoutine =
                MoveToIndexRoutine(Index - 1, (1 - phase) * UiAnimationDuration, () => moveToIndexRoutine = null));
        }
    }

    protected override void MoveRight()
    {
        if (HasLevel(Index + 1) && RightBtn.Interactable && MobileInput.Condition)
        {
            float phase = Mathf.Abs(Offset - Index) / 1;
            
            if(moveToIndexRoutine != null)
                StopCoroutine(moveToIndexRoutine);
            StartCoroutine(moveToIndexRoutine =
                MoveToIndexRoutine(Index + 1, (1 - phase) * UiAnimationDuration, () => moveToIndexRoutine = null));
        }
    }

    void HideUI()
    {

        RightBtn.RectTransform.DOAnchorPos(new Vector2(210, 0), UiAnimationDuration).onComplete = () => RightBtn.SetActive(false);
        LeftBtn.RectTransform.DOAnchorPos(new Vector2(-210, 0), UiAnimationDuration).onComplete = () => LeftBtn.SetActive(false);
        
        Tweener interactBtnTweener = InteractBtn.RectTransform.DOAnchorPos(new Vector2(0, InteractBtn.RectTransform.rect.y - MainButtonsOffset), UiAnimationDuration);
        interactBtnTweener.onPlay = () => InteractBtn.Interactable = false; 
        interactBtnTweener.onComplete = () => InteractBtn.SetActive(false);
        
        HideCollectionButton(UiAnimationDuration);

        IsFocused = false;
    }

    void ShowUI()
    {
        RightBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        LeftBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        
        InteractBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f)
            .onPlay = () =>
        {
            InteractBtn.Interactable = true;
            InteractBtn.SetActive(true);
        };

        if(Current.CollectionEnabled)
            ShowCollectionButton(UiAnimationDuration, 0.25f);
        else
            HideCollectionButton(UiAnimationDuration);

        CreateLevel(Index);
        CreateLevel(Index - 1);
        CreateLevel(Index + 1);

        this.Invoke(() => IsFocused = true, UiAnimationDuration);
    }
    
    void BringBackUI(PlayerView _NewPlayer)
    {

        RightBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        LeftBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        
        Tweener interactBtnTweener = InteractBtn.RectTransform.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        interactBtnTweener.onPlay = () =>
        {
            InteractBtn.Interactable = true;
            InteractBtn.SetActive(true);
        };

        if(Current.CollectionEnabled)
            ShowCollectionButton(UiAnimationDuration, 0.25f);
        else
            HideCollectionButton(UiAnimationDuration);

        if (_NewPlayer != null)
        {    
            DestroyImmediate(m_PlayerView.gameObject);
            m_PlayerView = _NewPlayer;
            LevelContainer.GetComponentInChildren<LevelRootView>().PlayerView = m_PlayerView;
        }
        else
        {
            m_PlayerView.transform.DOMove(Vector3.zero, UiAnimationDuration).SetDelay(0.25f);
        }

        this.Invoke(() => IsFocused = true, UiAnimationDuration);
    }

    void HideActivePlayer()
    {
        m_PlayerView.transform.DOMove(Vector3.down * ScreenScaler.CameraSize.y, UiAnimationDuration);
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

    void OnInteract()
    {
        LauncherUI.Instance.InvokePlayLauncher(new PlayLauncherEventArgs(Current));
        
        CleanContainers();
        
        IsFocused = false;
    }

    void CleanContainers()
    {
        int count = levelContainers.Count;
        
        for (int i = 0; i < count; i++)
        {
            if (i != Index)
            {
                Destroy(levelContainers[i].gameObject);
                levelContainers.Remove(i);
            }
        }
    }

    void OnCollection()
    {
        ShowCollection();
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
        
        Transform level = Instantiate(Selection[index].LevelRootPrefab, LevelContainer).transform;
        level.localPosition = index * ScreenScaler.CameraSize * Vector2.right;
        levelContainers[index] = level.GetComponent<LevelRootView>();
    }

    IEnumerator TimedAfterTouchRoutine(float duration, Action finished = null)
    {
        float startOffset = Offset;
        int targetIndex = Mathf.RoundToInt(startOffset);

        float time = 0;
        
        while (time <= duration)
        {
            Offset = Mathf.Lerp(startOffset, targetIndex, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        Index = targetIndex;
        finished?.Invoke();
    }

    IEnumerator MoveToIndexRoutine(int index, float duration, Action finished = null)
    {
        float time = 0;
        float startOffset = Offset;
        while (time < duration)
        {
            Offset = Mathf.Lerp(startOffset, index, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        Index = index;
        finished?.Invoke();
    } 
    
    #region Offset
    
    protected override void ProcessOffset()
    {
        LevelContainer.position = - Offset * ScreenScaler.CameraSize.x * Vector3.right;

        ProcessLevels();
        ProcessColors();
        ProcessButtons();
        
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

        int direction = Index > nextLevel ? 1 : -1;

        Color startLeftBtnColor = HasLevel(Index - 1) ? Color.white : Color.clear;
        Color startRightBtnColor = HasLevel(Index + 1) ? Color.white : Color.clear;
        
        Color targetLeftBtnColor = HasLevel(Index - direction - 1) ? Color.white : Color.clear;
        Color targetRightBtnColor = HasLevel(Index - direction + 1) ? Color.white : Color.clear;

        LeftBtn.Color = Color.Lerp(startLeftBtnColor, targetLeftBtnColor, phase);
        RightBtn.Color = Color.Lerp(startRightBtnColor, targetRightBtnColor, phase);

        //TODO
        Vector2 startCollectionBtnSizeFactor = CollectionBtn.RectTransform.GetAnchorsSize();
        Vector2 targetCollectionBtnSizeFactor = CollectionBtn.RectTransform.GetAnchorsSize();

        startCollectionBtnSizeFactor *= Current.CollectionEnabled ? Vector2.one : new Vector2(0, 1);
        targetCollectionBtnSizeFactor *= Selection[NextLevel].CollectionEnabled ? Vector2.one : new Vector2(0, 1);
        
        CollectionBtn.RectTransform.sizeDelta = Vector2.Lerp(startCollectionBtnSizeFactor, targetCollectionBtnSizeFactor, phase);    
            

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
    
    

    bool HasLevel(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < Length;
    }

    #endregion
    
    #region Index

    void ProcessIndex()
    {
        if(afterTouchRoutine != null)
            StopCoroutine(afterTouchRoutine);

        if(moveToIndexRoutine != null)
            StopCoroutine(moveToIndexRoutine);
        
        CreateLevel(Index - 1);
        CreateLevel(Index);
        CreateLevel(Index + 1);

        if(levelContainers.ContainsKey(Index))
            m_PlayerView = levelContainers[Index].PlayerView;
        else
            Debug.LogError("Key doesn't exists " + Index);
        
        LauncherUI.Instance.InvokeLevelChanged(new LevelChangedEventArgs(m_PlayerView, Current));
        
        Offset = Index;
        
        LevelContainer.position = - Index * ScreenScaler.CameraSize.x * Vector3.right;

        ProcessSortingOrderByIndex();
        ProcessLevelsByIndex();
        ProcessNameByIndex();
        ProcessButtonsByIndex();
        ProcessColorsByIndex();
    }

    void ProcessSortingOrderByIndex()
    {
        levelContainers[Index].SetSortingPriorityHigh();
        
        if(levelContainers.ContainsKey(Index - 1))
            levelContainers[Index - 1].SetSortingPriorityLow();
        if(levelContainers.ContainsKey(Index + 1))
            levelContainers[Index + 1].SetSortingPriorityLow();
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

    void ProcessButtonsByIndex()
    {
        //Managing right button
        RightBtn.Interactable = Index + 1 < Length;

        //Managing left button
        LeftBtn.Interactable = Index > 0;
    }
    #endregion

    void ProcessColorsByIndex()
    {
        CollectionBtn.Color = Current.ColorScheme.ButtonColor;
        InteractBtn.Color = Current.ColorScheme.ButtonColor;
        LauncherUI.Instance.UiManager.Wallet.Text.color = Current.ColorScheme.TextColorLauncher;
        InteractBtn.TextField.color = Current.ColorScheme.TextColor;
        CollectionBtn.TextField.color = Current.ColorScheme.TextColor;
    }

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

        
        Debug.LogWarning($"Touch down at {position}");
    }

    protected override void OnTouchMove_Handler(Vector2 delta)
    {
        if (!IsFocused)
            return;
        
        float offsetDelta = - delta.x / ScreenScaler.ScreenSize.x * TouchSensitivity; 

        bool shouldMove = Offset + offsetDelta >= 0 && Offset + offsetDelta < Length - 1;
        
        if(shouldMove)
            Offset += offsetDelta;
        
        Debug.LogWarning($"Touch moved at {delta}, offset increased on {offsetDelta}, now offset is {Offset} ");
    }

    protected override void OnTouchCancel_Handler(Vector2 position)
    {
        if (!IsFocused)
            return;
        
        StartCoroutine(afterTouchRoutine = TimedAfterTouchRoutine(0.6f));
        
        Debug.LogWarning($"Touch ended at {position}");
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
