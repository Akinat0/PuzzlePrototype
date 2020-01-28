using Abu.Tools;
using ScreensScripts;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class SelectionManager : MonoBehaviour
{

    [Header("UI elements"),]

    [SerializeField] private Transform PlayerContainer;

    [SerializeField] private Transform BackgroundContainer;

    [SerializeField] private Text InteractBtnText;
    
    [SerializeField] private GameObject RightBtn;

    [SerializeField] private GameObject LeftBtn;

    [SerializeField] private GameObject InteractBtn;

    [Space(8), SerializeField, Tooltip("The Scriptable object with items")]
    private LevelConfig[] _Selection;

    public LevelConfig CurrentItem{ get; private set;}

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
            PlayerContainer.gameObject.SetActive(false);
            InteractBtn.SetActive(false);
            return;
        }

        CurrentItem = _Selection[_Index];

        PlayerView playerView = DisplayPlayer(_Direction);
        DisplayBackground(_Direction);

        InteractBtnText.text = CurrentItem.Name;
        
        //Managing right button
        RightBtn.SetActive(_Index + 1 != _Selection.Length);

        //Managing left button
        LeftBtn.SetActive(_Index != 0);
        
        LauncherUI.Instance.InvokeLevelChanged(new LevelChangedEventArgs(playerView, CurrentItem));
        
        if (_Selection.Length == 0)
        {
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);
        }
        
    }

    PlayerView DisplayPlayer(int _Direction)
    {
        Vector2 camSize = ScreenScaler.CameraSize();
        
        PlayerView oldPrefab = PlayerContainer.GetComponentInChildren<PlayerView>();

        if (oldPrefab != null && _Direction != 0)
        {
            oldPrefab.transform.DOMove(new Vector3(camSize.x * Mathf.Sign(_Direction), 0), 0.5f).onComplete = () => Destroy(oldPrefab.gameObject);

            //Create new prefab
            PlayerView playerView = Instantiate(CurrentItem.PlayerPrefab, PlayerContainer).GetComponent<PlayerView>();

            playerView.transform.position += new Vector3(-camSize.x * Mathf.Sign(_Direction), 0);
            playerView.transform.DOMove(Vector3.zero, 0.5f);

            return playerView;
        }
        else
        {
            //Create new prefab
            PlayerView playerView = Instantiate(CurrentItem.PlayerPrefab, PlayerContainer).GetComponent<PlayerView>();
            return playerView;
        }
    }

    void DisplayBackground(int _Direction)
    {
        Vector2 camSize = ScreenScaler.CameraSize();
        
        BackgroundView oldPrefab = BackgroundContainer.GetComponentInChildren<BackgroundView>();

        if (oldPrefab != null && _Direction != 0)
        {
            oldPrefab.transform.DOMove(new Vector3(camSize.x * Mathf.Sign(_Direction), 0), 0.25f).onComplete = () => Destroy(oldPrefab.gameObject);
            
            //Create new prefab
            BackgroundView backgroundView = Instantiate(CurrentItem.BackgroundPrefab, BackgroundContainer).GetComponent<BackgroundView>();

            
            backgroundView.transform.position += new Vector3(-camSize.x * Mathf.Sign(_Direction), 0);
            backgroundView.transform.DOMove(Vector3.zero, 0.25f);
        }
        else
        {
            //Create new prefab
            BackgroundView backgroundView = Instantiate(CurrentItem.BackgroundPrefab, BackgroundContainer).GetComponent<BackgroundView>();
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
        DisplayItem(ItemNumber);
    }

    public void Interact()
    {
        LauncherUI.Instance.InvokePlayLauncher(new PlayLauncherEventArgs(CurrentItem));
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
