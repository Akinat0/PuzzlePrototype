using System;
using ScreensScripts;
using UnityEngine.UI;
using UnityEngine;


public class SelectionManager : MonoBehaviour
{

    [Header("UI elements"), SerializeField, Tooltip("Canvas representing selector")]
    private GameObject SelectorCanvas; //Canvas representing shop

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
        DisplayItem(ItemNumber);
    }
    
    //Called when left btn clicks
    public void OnLeftBtnClick()
    {
        ItemNumber--;
        DisplayItem(ItemNumber);
    }

    private void DisplayItem(int index)
    {
        if (_Selection.Length == 0) //If nothing remains in shop 
        {
            PlayerContainer.gameObject.SetActive(false);
            InteractBtn.SetActive(false);
            return;
        }

        CurrentItem = _Selection[index];

        PlayerView playerView = DisplayPlayer();
        DisplayBackground();

        InteractBtnText.text = CurrentItem.Name;
        
        //Managing right button
        RightBtn.SetActive(index + 1 != _Selection.Length);

        //Managing left button
        LeftBtn.SetActive(index != 0);
        
        LauncherUI.Instance.InvokeLevelChanged(new LevelChangedEventArgs(playerView, CurrentItem));
        
        if (_Selection.Length == 0)
        {
            LeftBtn.SetActive(false);
            RightBtn.SetActive(false);
        }
        
    }

    PlayerView DisplayPlayer()
    {
        //Delete old prefab
        foreach (Transform child in PlayerContainer.GetComponentInChildren<Transform>())
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
        //Create new prefab
        PlayerView playerView = Instantiate(CurrentItem.PlayerPrefab, PlayerContainer).GetComponent<PlayerView>();

        return playerView;
    }

    void DisplayBackground()
    {
        //Delete old container
        foreach (Transform child in BackgroundContainer.GetComponentInChildren<Transform>())
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
        //Create new Background
        Instantiate(CurrentItem.BackgroundPrefab, BackgroundContainer);
    }

    void HideUI()
    {
        RightBtn.SetActive(false);
        LeftBtn.SetActive(false);
        InteractBtn.SetActive(false);
    }

    void ShowUI()
    {
        InteractBtn.SetActive(true);
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
