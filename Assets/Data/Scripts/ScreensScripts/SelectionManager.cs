using Abu.Tools;
using ScreensScripts;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class SelectionManager : MonoBehaviour
{

    [Header("UI elements"),]
    
    [SerializeField] private Transform LevelContainer;

    [SerializeField] private Text InteractBtnText;
    
    [SerializeField] private GameObject RightBtn;

    [SerializeField] private GameObject LeftBtn;

    [SerializeField] private GameObject InteractBtn;

    [Space(8), SerializeField, Tooltip("The Scriptable object with items")]
    private LevelConfig[] _Selection;

    public LevelConfig CurrentItem{ get; private set;}


    private PlayerView m_PlayerView;
    private BackgroundView m_BackgroundView;
    private int ItemNumber; //Index representing current item in the shop
    
    public void Start()
    {
        ItemNumber = 0;
        DisplayItem(ItemNumber);
    }
    
    //Called when right btn clicks
    public void OnRightBtnClick()
    {
        ItemNumber++;
        DisplayItem(ItemNumber, -1);
    }
    
    //Called when left btn clicks
    public void OnLeftBtnClick()
    {
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

        DisplayLevel(_Direction);

        InteractBtnText.text = CurrentItem.Name;
        
        //Managing right button
        RightBtn.SetActive(_Index + 1 != _Selection.Length);

        //Managing left button
        LeftBtn.SetActive(_Index != 0);
        
        LauncherUI.Instance.InvokeLevelChanged(new LevelChangedEventArgs(m_PlayerView, CurrentItem));
        
        if (_Selection.Length == 0)
        {
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);
        }
        
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
        Vector2 camSize = ScreenScaler.CameraSize();
        
        PlayerView oldPrefab = null;

        if(_OldLevelView != null)
            oldPrefab = _OldLevelView.GetComponentInChildren<PlayerView>();

        if (_Direction != 0 && oldPrefab != null)
        {
            var tweenerPlayer = oldPrefab.transform.DOMove(new Vector3(camSize.x * Mathf.Sign(_Direction), 0), 0.5f);
            tweenerPlayer.onPlay = () =>
            {
                RightBtn.GetComponent<Button>().interactable = false;
                LeftBtn.GetComponent<Button>().interactable = false;
            };

            tweenerPlayer.onComplete = () =>
            {
                // It takes a bit more time for player to finish animation,
                // so we will destroy old level prefab in player's finish animation code
                if(_OldLevelView != null)
                    Destroy(_OldLevelView.gameObject); 
                RightBtn.GetComponent<Button>().interactable = true;
                LeftBtn.GetComponent<Button>().interactable = true;
            };

            //Create new prefab
            PlayerView playerView = _PlayerPrefab.GetComponent<PlayerView>();

            playerView.transform.position += new Vector3(-camSize.x * Mathf.Sign(_Direction), 0);
            playerView.transform.DOMove(Vector3.zero, 0.5f);

            return playerView;
        }
        else
        {
            return _PlayerPrefab.GetComponent<PlayerView>();;
        }
    }

    BackgroundView DisplayBackground(int _Direction, GameObject _BackgroundPrefab, Transform _OldLevelView = null)
    {
        Vector2 camSize = ScreenScaler.CameraSize();
        
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
        RectTransform rightBtnRect = RightBtn.GetComponent<RectTransform>();
        RectTransform interactBtnRect = InteractBtn.GetComponent<RectTransform>();
        RectTransform leftBtnRect = LeftBtn.GetComponent<RectTransform>();

        rightBtnRect.DOAnchorPos(new Vector2(210, 0), 0.3f).onComplete = () => RightBtn.SetActive(false);
        leftBtnRect.DOAnchorPos(new Vector2(-210, 0), 0.3f).onComplete = () => LeftBtn.SetActive(false);
        
        Tweener interactBtnTweener = interactBtnRect.DOAnchorPos(new Vector2(0, -290), 0.3f);
        interactBtnTweener.onPlay = () => InteractBtn.GetComponent<Button>().interactable = false;
        interactBtnTweener.onComplete = () => InteractBtn.SetActive(false);
    }

    void ShowUI()
    {
        RectTransform rightBtnRect = RightBtn.GetComponent<RectTransform>();
        RectTransform interactBtnRect = InteractBtn.GetComponent<RectTransform>();
        RectTransform leftBtnRect = LeftBtn.GetComponent<RectTransform>();

        rightBtnRect.DOAnchorPos(Vector2.zero, 0.3f).SetDelay(0.25f);
        leftBtnRect.DOAnchorPos(Vector2.zero, 0.3f).SetDelay(0.25f);
        
        Tweener interactBtnTweener = interactBtnRect.DOAnchorPos(Vector2.zero, 0.3f).SetDelay(0.25f);
        interactBtnTweener.onPlay = () =>
        {
            InteractBtn.GetComponent<Button>().interactable = true;
            InteractBtn.SetActive(true);
        };

        ClearContainers();
        DisplayItem(ItemNumber);
    }

    public void Interact()
    {
        LauncherUI.Instance.InvokePlayLauncher(new PlayLauncherEventArgs(CurrentItem));
    }

    private void ClearContainers()
    {
        foreach (Transform go in LevelContainer.transform)
            Destroy(go.gameObject, 0);
    }

    private void OnEnable()
    {
        LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
        LauncherUI.GameSceneUnloadedEvent += GameSceneUnloadedEvent_Handler;
    }

    private void OnDisable()
    {
        LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
        LauncherUI.GameSceneUnloadedEvent -= GameSceneUnloadedEvent_Handler;
    }

    private void PlayLauncherEvent_Handler(PlayLauncherEventArgs _Args)
    {
        HideUI();
    }    

    private void GameSceneUnloadedEvent_Handler()
    {
        ShowUI();   
    }

   
}
