using Abu.Tools;
using Abu.Tools.UI;
using Data.Scripts.Tools.Input;
using ScreensScripts;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class LevelSelectorComponent : SelectorComponent<LevelConfig>
{
    [SerializeField] Transform LevelContainer;
    [SerializeField] TextButtonComponent InteractBtn;
    [SerializeField] TextButtonComponent CollectionBtn;
    
    PlayerView m_PlayerView;

    //TODO what's it?
    BoolToggle m_ShowPlayerAnimated = new BoolToggle(false);

    protected override int Index
    {
        get => base.Index;
        set
        {
            base.Index = value;
            Account.DefaultLevelId = value;
        }
    }

    //Constants
    public const float MainButtonsOffset = 500; 
    public const float PlayerAnimationDuration = 0.5f; 
    public const float UiAnimationDuration = 0.5f;

    public void Start()
    {
        Selection = Account.LevelConfigs;
        Index = Account.DefaultLevelId;
        ShowUI();
    }
    
    protected override void MoveLeft()
    {
        if (Index == 0 || !LeftBtn.gameObject.activeInHierarchy || !LeftBtn.Interactable || !MobileInput.Condition)
        {
            Debug.Log("Selection's already on the first element or left button disabled");
            return;
        }
        
        Index--;
        DisplayItem(1);
    }

    protected override void MoveRight()
    {
        if (Index == Length - 1 || !RightBtn.gameObject.activeInHierarchy || !RightBtn.Interactable || !MobileInput.Condition)
        {
            Debug.Log("Selection's already on the last element or right button disabled");
            return;
        }

        Index++;
        DisplayItem(-1);
    }

    private void DisplayItem(int _Direction = 0)
    {
        if (Selection.Length == 0) //If nothing remains in shop 
        {
            LevelContainer.gameObject.SetActive(false);
            InteractBtn.SetActive(false);
            return;
        }

        if(_Direction == 0)
            SetupColors();
        
        DisplayLevel(_Direction);

        InteractBtn.Text = Current.Name;

        ManagingButtons();
        
        LauncherUI.Instance.InvokeLevelChanged(new LevelChangedEventArgs(m_PlayerView, Current));
        
        if (Length == 0)
        {
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);
        }
    }

    void ManagingButtons()
    {
        //Managing right button
        RightBtn.SetActive(Index + 1 != Length);

        //Managing left button
        LeftBtn.SetActive(Index != 0);
    }

    LevelRootView DisplayLevel(int _Direction)
    {
        LevelRootView oldPrefab = LevelContainer.GetComponentInChildren<LevelRootView>();

        if (oldPrefab != null && _Direction != 0)
        {
            LevelRootView levelRootView = Instantiate(Current.LevelRootPrefab, LevelContainer).GetComponent<LevelRootView>();
            m_PlayerView = DisplayPlayer(_Direction, levelRootView.PlayerView.gameObject, oldPrefab.transform);
            DisplayBackground(_Direction, levelRootView.BackgroundView.gameObject, oldPrefab.transform);
            return levelRootView;
        }
        else
        {
            LevelRootView levelRootView = Instantiate(Current.LevelRootPrefab, LevelContainer).GetComponent<LevelRootView>();
            m_PlayerView = DisplayPlayer(_Direction, levelRootView.PlayerView.gameObject);
            DisplayBackground(_Direction, levelRootView.BackgroundView.gameObject);
            return levelRootView;
        }
    }

    PlayerView DisplayPlayer(int _Direction, GameObject _PlayerPrefab, Transform _OldLevelView = null)
    {
        Vector2 camSize = ScreenScaler.CameraSize;
        
        PlayerView oldPrefab = null;

        if(_OldLevelView != null)
            oldPrefab = _OldLevelView.GetComponentInChildren<PlayerView>();

        if (Current.CollectionEnabled)
        {
            GameObject defaultCollectionPlayer = Instantiate(Account.CollectionDefaultItem.GetPuzzleVariant(Current.PuzzleSides), _PlayerPrefab.transform.parent, true);
            DestroyImmediate(_PlayerPrefab);
            _PlayerPrefab = defaultCollectionPlayer;
            ShowCollectionButton(PlayerAnimationDuration);
        }
        else 
            HideCollectionButton(_Direction == 0 ? 0 : PlayerAnimationDuration);
        
        if (_Direction != 0 && oldPrefab != null)
        {
            var tweenerPlayer = oldPrefab.transform.DOMove(new Vector3(camSize.x * Mathf.Sign(_Direction), 0), PlayerAnimationDuration);
            tweenerPlayer.onPlay = () =>
            {
                MobileInput.Condition = false;
                RightBtn.Interactable = false;
                LeftBtn.Interactable = false;
            };

            tweenerPlayer.onComplete = () =>
            {
                // It takes a bit more time for player to finish animation,
                // so we will destroy old level prefab in player's finish animation code
                if(_OldLevelView != null)
                    Destroy(_OldLevelView.gameObject);
                MobileInput.Condition = true;
                RightBtn.Interactable = true;
                LeftBtn.Interactable = true;
            };

            //Create new prefab
            PlayerView playerView = _PlayerPrefab.GetComponent<PlayerView>();

            playerView.transform.position += new Vector3(-camSize.x * Mathf.Sign(_Direction), 0);
            playerView.transform.DOMove(Vector3.zero, PlayerAnimationDuration);

            SetupColors(PlayerAnimationDuration);

            return playerView;
        }
        else
        {
            if (m_ShowPlayerAnimated.Value)
            {
                _PlayerPrefab.transform.localPosition += Vector3.down * camSize.y;
                _PlayerPrefab.transform.DOMove(Vector3.zero, UiAnimationDuration).SetDelay(0.25f);
            }
            return _PlayerPrefab.GetComponent<PlayerView>();
        }
    }

    BackgroundView DisplayBackground(int _Direction, GameObject _BackgroundPrefab, Transform _OldLevelView = null)
    {
        Vector2 camSize = ScreenScaler.CameraSize;
        
        BackgroundView oldPrefab = null;

        if(_OldLevelView != null)
            oldPrefab = _OldLevelView.GetComponentInChildren<BackgroundView>();
        
        if (_Direction != 0 && oldPrefab != null)
        {

            oldPrefab.transform.DOMove(new Vector3(camSize.x * Mathf.Sign(_Direction), 0), 0.25f);
            BackgroundView backgroundView = _BackgroundPrefab.GetComponent<BackgroundView>();

            backgroundView.transform.position += new Vector3(-camSize.x * Mathf.Sign(_Direction), 0);
            backgroundView.transform.DOMove(Vector3.zero, 0.25f);

            return backgroundView;
        }
        else
        {
            //Create new prefab
            return _BackgroundPrefab.GetComponent<BackgroundView>();
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
        
        ClearContainers();
        
        DisplayItem();
        
        this.Invoke(() => IsFocused = true, UiAnimationDuration);
    }
    
    void BringBackUI(PlayerView _NewPlayer)
    {
        ManagingButtons();

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

    void SetupColors(float _Duration = 0)
    {
        LevelColorScheme colorScheme = Current.ColorScheme;

        if (_Duration > 0)
        {
            DOTween.To(() => LeftBtn.Color,
                x => LeftBtn.Color = x, colorScheme.ArrowColor, _Duration);

            DOTween.To(() => RightBtn.Color,
                x => RightBtn.Color = x, colorScheme.ArrowColor, _Duration);

            DOTween.To(() => InteractBtn.Color,
                x => InteractBtn.Color = x, colorScheme.ButtonColor, _Duration);
            
            DOTween.To(() => InteractBtn.TextField.color,
                x => InteractBtn.TextField.color = x, colorScheme.TextColor, _Duration);
            
            DOTween.To(() => CollectionBtn.Color,
                x => CollectionBtn.Color = x, colorScheme.ButtonColor, _Duration);
            
            DOTween.To(() => CollectionBtn.TextField.color,
                x => CollectionBtn.TextField.color = x, colorScheme.TextColor, _Duration);
        }
        else
        {
            LeftBtn.Color = colorScheme.ArrowColor;
            RightBtn.Color = colorScheme.ArrowColor;
            InteractBtn.Color = colorScheme.ButtonColor;
            InteractBtn.TextField.color = colorScheme.TextColor;
            CollectionBtn.Color = colorScheme.ButtonColor;
            CollectionBtn.TextField.color = colorScheme.TextColor;
        }
    }
    void ClearContainers()
    {
        foreach (Transform go in LevelContainer.transform)
            Destroy(go.gameObject, 0);
    }

    void OnInteract()
    {
        LauncherUI.Instance.InvokePlayLauncher(new PlayLauncherEventArgs(Current));
    }

    void OnCollection()
    {
        ShowCollection();
    }

    protected override void OnSwipeDown()
    {
        base.OnSwipeDown();
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
}
