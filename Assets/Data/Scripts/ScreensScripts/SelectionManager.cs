using Abu.Tools;
using Abu.Tools.UI;
using ScreensScripts;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SelectionManager : MonoBehaviour
{

    [Header("UI elements"),] [SerializeField]
    private Transform LevelContainer;

    [SerializeField] private ButtonComponent RightBtn;

    [SerializeField] private ButtonComponent LeftBtn;

    [SerializeField] private TextButtonComponent InteractBtn;

    [SerializeField] private TextButtonComponent CollectionBtn;

    [Space(8), SerializeField, Tooltip("The Scriptable object with items")]
    private LevelConfig[] _Selection;

    public LevelConfig CurrentItem{ get; private set;}
    
    
    private PlayerView m_PlayerView;
    private int ItemNumber; //Index representing current item in the shop

    private BoolToggle m_ShowPlayerAnimated = new BoolToggle(false);
    private MobileSwipeInputComponent MobileSwipeInputComponent;

    //Constants
    public const float MainButtonsOffset = 500; 
    public const float PlayerAnimationDuration = 0.5f; 
    public const float UiAnimationDuration = 0.5f; 
    
    public void Awake()
    {
        MobileSwipeInputComponent = GetComponent<MobileSwipeInputComponent>();
    }
    
    public void Start()
    {
        ItemNumber = 0;
        DisplayItem(ItemNumber);
    }
    
    //Called when right btn clicks
    public void OnRightBtnClick()
    {
        if (ItemNumber == _Selection.Length - 1 || !RightBtn.gameObject.activeInHierarchy || !RightBtn.Interactable)
        {
            Debug.Log("Selection's already on the last element or right button disabled");
            return;
        }

        ItemNumber++;
        DisplayItem(ItemNumber, -1);
    }
    
    //Called when left btn clicks
    public void OnLeftBtnClick()
    {
        if (ItemNumber == 0 || !LeftBtn.gameObject.activeInHierarchy || !LeftBtn.Interactable)
        {
            Debug.Log("Selection's already on the first element or left button disabled");
            return;
        }
        
        ItemNumber--;
        DisplayItem(ItemNumber, 1);
    }

    private void DisplayItem(int _Index, int _Direction = 0)
    {
        if (_Selection.Length == 0) //If nothing remains in shop 
        {
            LevelContainer.gameObject.SetActive(false);
            InteractBtn.SetActive(false);
            return;
        }

        CurrentItem = _Selection[_Index];

        if(_Direction == 0)
            SetupColors();
        
        DisplayLevel(_Direction);

        InteractBtn.Text = CurrentItem.Name;

        ManagingButtons();
        
        LauncherUI.Instance.InvokeLevelChanged(new LevelChangedEventArgs(m_PlayerView, CurrentItem));
        
        if (_Selection.Length == 0)
        {
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);
        }
        
    }

    void ManagingButtons()
    {
        //Managing right button
        RightBtn.SetActive(ItemNumber + 1 != _Selection.Length);

        //Managing left button
        LeftBtn.SetActive(ItemNumber != 0);
    }

    LevelRootView DisplayLevel(int _Direction)
    {
        LevelRootView oldPrefab = LevelContainer.GetComponentInChildren<LevelRootView>();

        if (oldPrefab != null && _Direction != 0)
        {
            LevelRootView levelRootView = Instantiate(CurrentItem.LevelRootPrefab, LevelContainer).GetComponent<LevelRootView>();
            m_PlayerView = DisplayPlayer(_Direction, levelRootView.PlayerView.gameObject, oldPrefab.transform);
            DisplayBackground(_Direction, levelRootView.BackgroundView.gameObject, oldPrefab.transform);
            return levelRootView;
        }
        else
        {
            LevelRootView levelRootView = Instantiate(CurrentItem.LevelRootPrefab, LevelContainer).GetComponent<LevelRootView>();
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

        if (CurrentItem.CollectionEnabled)
        {
            GameObject defaultCollectionPlayer = Instantiate(Account.CollectionDefaultItem.GetPuzzleVariant(CurrentItem.PuzzleSides), _PlayerPrefab.transform.parent, true);
            DestroyImmediate(_PlayerPrefab);
            _PlayerPrefab = defaultCollectionPlayer;
            ShowCollectionButton(PlayerAnimationDuration);
        }
        else
            HideCollectionButton(PlayerAnimationDuration);
        
        if (_Direction != 0 && oldPrefab != null)
        {
            var tweenerPlayer = oldPrefab.transform.DOMove(new Vector3(camSize.x * Mathf.Sign(_Direction), 0), PlayerAnimationDuration);
            tweenerPlayer.onPlay = () =>
            {
                RightBtn.Interactable = false;
                LeftBtn.Interactable = false;
            };

            tweenerPlayer.onComplete = () =>
            {
                // It takes a bit more time for player to finish animation,
                // so we will destroy old level prefab in player's finish animation code
                if(_OldLevelView != null)
                    Destroy(_OldLevelView.gameObject); 
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
    }

    void ShowUI()
    {
        RectTransform rightBtnRect = RightBtn.GetComponent<RectTransform>();
        RectTransform leftBtnRect = LeftBtn.GetComponent<RectTransform>();
        RectTransform interactBtnRect = InteractBtn.GetComponent<RectTransform>();
        
        rightBtnRect.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        leftBtnRect.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        
        Tweener interactBtnTweener = interactBtnRect.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        interactBtnTweener.onPlay = () =>
        {
            InteractBtn.Interactable = true;
            InteractBtn.SetActive(true);
        };

        if(CurrentItem.CollectionEnabled)
            ShowCollectionButton(UiAnimationDuration, 0.25f);
        else
            HideCollectionButton(UiAnimationDuration);
        
        ClearContainers();
        
        DisplayItem(ItemNumber);
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

        if(CurrentItem.CollectionEnabled)
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

    }

    void SetPlayerFromCollection()
    {
        DestroyImmediate(m_PlayerView);
      //  m_PlayerView = Instantiate();
    }
    
    void HideActivePlayer()
    {
        m_PlayerView.transform.DOMove(Vector3.down * ScreenScaler.CameraSize.y, UiAnimationDuration);
    }
    
    void ShowActivePlayer()
    {
        m_PlayerView.transform.DOMove(Vector3.zero, UiAnimationDuration);
    }


    public void OnInteract()
    {
        LauncherUI.Instance.InvokePlayLauncher(new PlayLauncherEventArgs(CurrentItem));
    }

    public void OnCollection()
    {
        LauncherUI.Instance.InvokeShowCollection(new ShowCollectionEventArgs(CurrentItem.ColorScheme));
        HideUI();
        HideActivePlayer();
    }
    
    private void ShowCollectionButton(float _Duration = 0, float _Delay = 0)
    {
        CollectionBtn.SetActive(true);
        CollectionBtn.Interactable = true;
        
        CollectionBtn.RectTransform.DOAnchorPos(Vector2.zero, _Duration).SetDelay(_Delay);
    }
    
    private void HideCollectionButton(float _Duration)
    {
        RectTransform collectionBtnRect = CollectionBtn.GetComponent<RectTransform>();

        collectionBtnRect.DOAnchorPos(new Vector2(0, collectionBtnRect.rect.y - MainButtonsOffset), _Duration)
            .OnStart(() =>
            {
                CollectionBtn.Interactable = false;
            })
            .SetUpdate(true)
            .onComplete = () => CollectionBtn.SetActive(false);
    }

    private void SetupColors(float _Duration = 0)
    {
        LevelColorScheme colorScheme = CurrentItem.ColorScheme;

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
            LeftBtn.GetComponent<Image>().color = colorScheme.ArrowColor;
            RightBtn.GetComponent<Image>().color = colorScheme.ArrowColor;
            InteractBtn.GetComponent<Image>().color = colorScheme.ButtonColor;
            InteractBtn.TextField.color = colorScheme.TextColor;
            CollectionBtn.GetComponent<Image>().color = colorScheme.ButtonColor;
            CollectionBtn.TextField.color = colorScheme.TextColor;
        }
    }
    private void ClearContainers()
    {
        foreach (Transform go in LevelContainer.transform)
            Destroy(go.gameObject, 0);
    }

    private void OnEnable()
    {
        if (MobileSwipeInputComponent != null)
            MobileSwipeInputComponent.OnSwipe += MobileSwipeEvent_handler;
        LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
        LauncherUI.GameSceneUnloadedEvent += GameSceneUnloadedEvent_Handler;
        LauncherUI.CloseCollectionEvent += CloseCollectionEvent_Handler;
        
        RightBtn.OnClick += OnRightBtnClick;
        LeftBtn.OnClick += OnLeftBtnClick;
        InteractBtn.OnClick += OnInteract;
        CollectionBtn.OnClick += OnCollection;
    }

    private void OnDisable()
    {
        if (MobileSwipeInputComponent != null)
            MobileSwipeInputComponent.OnSwipe -= MobileSwipeEvent_handler;
        LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
        LauncherUI.GameSceneUnloadedEvent -= GameSceneUnloadedEvent_Handler;
        LauncherUI.CloseCollectionEvent -= CloseCollectionEvent_Handler;
        
        RightBtn.OnClick -= OnRightBtnClick;
        LeftBtn.OnClick -= OnLeftBtnClick;
        InteractBtn.OnClick -= OnInteract;
        CollectionBtn.OnClick -= OnCollection;
    }

    private void MobileSwipeEvent_handler(SwipeType swipe)
    {
        switch (swipe)
        {
            case SwipeType.Left:
                OnRightBtnClick();
                break;
            
            case SwipeType.Right:
                OnLeftBtnClick();
                break;
        }
    }
    
    private void PlayLauncherEvent_Handler(PlayLauncherEventArgs _Args)
    {
        HideUI();
    }

    private void GameSceneUnloadedEvent_Handler()
    {
        ShowUI();   
    }

    //The handler handles two behaviours: if we chose new player or if we not
    private void CloseCollectionEvent_Handler(CloseCollectionEventArgs _Args)
    {
        BringBackUI(_Args.PlayerView);
    }

}
