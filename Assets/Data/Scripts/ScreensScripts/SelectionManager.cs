using Abu.Tools;
using ScreensScripts;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SelectionManager : MonoBehaviour
{

    [Header("UI elements"),] [SerializeField]
    private Transform LevelContainer;

    [SerializeField] private GameObject RightBtnObject;

    [SerializeField] private GameObject LeftBtnObject;

    [SerializeField] private GameObject InteractBtnObject;

    [SerializeField] private TextMeshProUGUI InteractBtnText;

    [SerializeField] private GameObject CollectionBtnObject;

    [SerializeField] private TextMeshProUGUI CollectionBtnText;

    [Space(8), SerializeField, Tooltip("The Scriptable object with items")]
    private LevelConfig[] _Selection;

    public LevelConfig CurrentItem{ get; private set;}

    //Buttons
    private Button rightBtn;
    private Button leftBtn;
    private Button interactBtn;
    private Button collectionBtn;
    
    private PlayerView m_PlayerView;
    private BackgroundView m_BackgroundView;
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
        rightBtn = RightBtnObject.GetComponent<Button>();
        leftBtn = LeftBtnObject.GetComponent<Button>();
        interactBtn = InteractBtnObject.GetComponent<Button>();
        collectionBtn = CollectionBtnObject.GetComponent<Button>();
        
        ItemNumber = 0;
        DisplayItem(ItemNumber);
    }
    
    //Called when right btn clicks
    public void OnRightBtnClick()
    {
        if (ItemNumber == _Selection.Length - 1 || !RightBtnObject.activeInHierarchy || !rightBtn.interactable)
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
        if (ItemNumber == 0 || !LeftBtnObject.activeInHierarchy || !leftBtn.interactable)
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
            InteractBtnObject.SetActive(false);
            return;
        }

        CurrentItem = _Selection[_Index];

        if(_Direction == 0)
            SetupColors();
        
        DisplayLevel(_Direction);

        InteractBtnText.text = CurrentItem.Name;

        ManagingButtons();
        
        LauncherUI.Instance.InvokeLevelChanged(new LevelChangedEventArgs(m_PlayerView, CurrentItem));
        
        if (_Selection.Length == 0)
        {
            LeftBtnObject.SetActive(false);
            RightBtnObject.SetActive(false);
        }
        
    }

    void ManagingButtons()
    {
        //Managing right button
        RightBtnObject.SetActive(ItemNumber + 1 != _Selection.Length);

        //Managing left button
        LeftBtnObject.SetActive(ItemNumber != 0);
    }

    LevelRootView DisplayLevel(int _Direction)
    {
        LevelRootView oldPrefab = LevelContainer.GetComponentInChildren<LevelRootView>();

        if (oldPrefab != null && _Direction != 0)
        {
            LevelRootView levelRootView = Instantiate(CurrentItem.LevelRootPrefab, LevelContainer).GetComponent<LevelRootView>();
            m_PlayerView = DisplayPlayer(_Direction, levelRootView.PlayerView.gameObject, oldPrefab.transform);
            m_BackgroundView = DisplayBackground(_Direction, levelRootView.BackgroundView.gameObject, oldPrefab.transform);
            return levelRootView;
        }
        else
        {
            LevelRootView levelRootView = Instantiate(CurrentItem.LevelRootPrefab, LevelContainer).GetComponent<LevelRootView>();
            m_PlayerView = DisplayPlayer(_Direction, levelRootView.PlayerView.gameObject);
            m_BackgroundView = DisplayBackground(_Direction, levelRootView.BackgroundView.gameObject);
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
                rightBtn.interactable = false;
                leftBtn.interactable = false;
            };

            tweenerPlayer.onComplete = () =>
            {
                // It takes a bit more time for player to finish animation,
                // so we will destroy old level prefab in player's finish animation code
                if(_OldLevelView != null)
                    Destroy(_OldLevelView.gameObject); 
                rightBtn.interactable = true;
                leftBtn.interactable = true;
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
        RectTransform rightBtnRect = RightBtnObject.GetComponent<RectTransform>();
        RectTransform leftBtnRect = LeftBtnObject.GetComponent<RectTransform>();
        RectTransform interactBtnRect = InteractBtnObject.GetComponent<RectTransform>();

        rightBtnRect.DOAnchorPos(new Vector2(210, 0), UiAnimationDuration).onComplete = () => RightBtnObject.SetActive(false);
        leftBtnRect.DOAnchorPos(new Vector2(-210, 0), UiAnimationDuration).onComplete = () => LeftBtnObject.SetActive(false);
        
        Tweener interactBtnTweener = interactBtnRect.DOAnchorPos(new Vector2(0, interactBtnRect.rect.y - MainButtonsOffset), UiAnimationDuration);
        interactBtnTweener.onPlay = () => interactBtn.interactable = false; 
        interactBtnTweener.onComplete = () => InteractBtnObject.SetActive(false);
        
        HideCollectionButton(UiAnimationDuration);
    }

    void ShowUI()
    {
        RectTransform rightBtnRect = RightBtnObject.GetComponent<RectTransform>();
        RectTransform leftBtnRect = LeftBtnObject.GetComponent<RectTransform>();
        RectTransform interactBtnRect = InteractBtnObject.GetComponent<RectTransform>();
        
        rightBtnRect.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        leftBtnRect.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        
        Tweener interactBtnTweener = interactBtnRect.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        interactBtnTweener.onPlay = () =>
        {
            interactBtn.interactable = true;
            InteractBtnObject.SetActive(true);
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
        
        RectTransform rightBtnRect = RightBtnObject.GetComponent<RectTransform>();
        RectTransform leftBtnRect = LeftBtnObject.GetComponent<RectTransform>();
        RectTransform interactBtnRect = InteractBtnObject.GetComponent<RectTransform>();
        
        rightBtnRect.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        leftBtnRect.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        
        Tweener interactBtnTweener = interactBtnRect.DOAnchorPos(Vector2.zero, UiAnimationDuration).SetDelay(0.25f);
        interactBtnTweener.onPlay = () =>
        {
            interactBtn.interactable = true;
            InteractBtnObject.SetActive(true);
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
        CollectionBtnObject.SetActive(true);
        collectionBtn.interactable = true;
        
        RectTransform collectionBtnRect = CollectionBtnObject.GetComponent<RectTransform>();
        collectionBtnRect.DOAnchorPos(Vector2.zero, _Duration).SetDelay(_Delay);
    }
    
    private void HideCollectionButton(float _Duration)
    {
        RectTransform collectionBtnRect = CollectionBtnObject.GetComponent<RectTransform>();

        collectionBtnRect.DOAnchorPos(new Vector2(0, collectionBtnRect.rect.y - MainButtonsOffset), _Duration)
            .OnStart(() =>
            {
                collectionBtn.interactable = false;
            })
            .SetUpdate(true)
            .onComplete = () => CollectionBtnObject.SetActive(false);
    }

    private void SetupColors(float _Duration = 0)
    {
        LevelColorScheme colorScheme = CurrentItem.ColorScheme;

        if (_Duration > 0)
        {
            Image leftBtnImage = LeftBtnObject.GetComponent<Image>();
            DOTween.To(() => leftBtnImage.color,
                x => leftBtnImage.color = x, colorScheme.ArrowColor, _Duration);

            Image rightBtnImage = RightBtnObject.GetComponent<Image>();
            DOTween.To(() => rightBtnImage.color,
                x => rightBtnImage.color = x, colorScheme.ArrowColor, _Duration);

            Image interactBtnImage = InteractBtnObject.GetComponent<Image>();
            DOTween.To(() => interactBtnImage.color,
                x => interactBtnImage.color = x, colorScheme.ButtonColor, _Duration);
            
            DOTween.To(() => InteractBtnText.color,
                x => InteractBtnText.color = x, colorScheme.TextColor, _Duration);
            
            Image collectionBtnImage = CollectionBtnObject.GetComponent<Image>();
            DOTween.To(() => collectionBtnImage.color,
                x => collectionBtnImage.color = x, colorScheme.ButtonColor, _Duration);
            
            DOTween.To(() => CollectionBtnText.color,
                x => CollectionBtnText.color = x, colorScheme.TextColor, _Duration);
        }
        else
        {
            LeftBtnObject.GetComponent<Image>().color = colorScheme.ArrowColor;
            RightBtnObject.GetComponent<Image>().color = colorScheme.ArrowColor;
            InteractBtnObject.GetComponent<Image>().color = colorScheme.ButtonColor;
            InteractBtnText.color = colorScheme.TextColor;
            CollectionBtnObject.GetComponent<Image>().color = colorScheme.ButtonColor;
            CollectionBtnText.color = colorScheme.TextColor;
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
    }

    private void OnDisable()
    {
        if (MobileSwipeInputComponent != null)
            MobileSwipeInputComponent.OnSwipe -= MobileSwipeEvent_handler;
        LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
        LauncherUI.GameSceneUnloadedEvent -= GameSceneUnloadedEvent_Handler;
        LauncherUI.CloseCollectionEvent -= CloseCollectionEvent_Handler;
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
