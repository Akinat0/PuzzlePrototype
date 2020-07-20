using System;
using Abu.Tools;
using Abu.Tools.UI;
using Data.Scripts.Tools.Input;
using DG.Tweening;
using ScreensScripts;
using UnityEngine;

public class CollectionSelectorComponent : SelectorComponent<CollectionItem>
{
    [SerializeField] TextButtonComponent InteractBtn;
    [SerializeField] ButtonComponent HomeBtn;
    [SerializeField] Transform ItemContainer;
    [SerializeField] RectTransform Content;

    PlayerView activePlayer;
    PlayerView oldPlayer;

    void Start()
    {
        Index = Account.CollectionDefaultItemId;
        Selection = Account.CollectionItems;
        HideCollection();
    }

    void DisplayItem(int _Index, int _Direction = 0)
    {

        CreateItem(_Index);

        Vector3 shift = _Direction * ScreenScaler.CameraSize.x * Vector3.right;
        
        activePlayer.transform.position = - shift;

        if(_Direction != 0)
        {
            oldPlayer.transform.DOMove(shift,
                    LevelSelectorComponent.PlayerAnimationDuration)
                .OnStart(() =>
                {
                    MobileInput.Condition = false;
                    RightBtn.Interactable = false;
                    LeftBtn.Interactable = false;
                })
                .OnComplete(() =>
                {
                    MobileInput.Condition = true;
                    RightBtn.Interactable = true;
                    LeftBtn.Interactable = true;
                    Destroy(oldPlayer.gameObject);
                });
            
            activePlayer.transform.DOMove(Vector3.zero, 
                LevelSelectorComponent.PlayerAnimationDuration);
        }
    }
    
    void CreateItem(int _Index)
    {
        oldPlayer = activePlayer;
        GameObject collectionPuzzle = Selection[_Index].GetPuzzleVariant(LauncherUI.Instance.LevelConfig.PuzzleSides);

        //TODO ignore unsupported puzzles
//        for (int i = 0; i < Length; i++)
//        {
//            collectionPuzzle = Selection[_Index].GetPuzzleVariant(LauncherUI.Instance.LevelConfig.PuzzleSides);
//            if (collectionPuzzle != null)
//                break;
//            
//            Debug.LogError($"Collection item {_Index} has no suitable variant");
//        }
//        
            
        activePlayer = Instantiate(collectionPuzzle, ItemContainer).GetComponent<PlayerView>();
    }

    void SetupColors(LevelColorScheme colorScheme)
    {
        LeftBtn.Color = colorScheme.ArrowColor;
        RightBtn.Color = colorScheme.ArrowColor;
        InteractBtn.Color = colorScheme.ButtonColor;
        HomeBtn.Color = colorScheme.ButtonColor;
        InteractBtn.TextField.color = colorScheme.TextColor;
    }
    
    void ShowCollection()
    {
        Content.gameObject.SetActive(true);
        DisplayItem(Index);

        activePlayer.transform.localPosition += Vector3.up * ScreenScaler.CameraSize.y;
        
        activePlayer.transform.DOMove(Vector3.zero, LevelSelectorComponent.UiAnimationDuration)
            .SetDelay(LevelSelectorComponent.UiAnimationDuration / 2);
        
        Content.DOAnchorPos(Vector2.zero, LevelSelectorComponent.UiAnimationDuration)
            .SetDelay(LevelSelectorComponent.UiAnimationDuration / 2);

        SetupColors(LauncherUI.Instance.LevelConfig.ColorScheme);
        
        this.Invoke(()=> IsFocused = true, LevelSelectorComponent.UiAnimationDuration);
    }
    
    void HideCollection(float _Duration = 0)
    {
        if (activePlayer != null)
        {
            activePlayer.transform.DOMove(Vector3.up * ScreenScaler.CameraSize.y,
                LevelSelectorComponent.UiAnimationDuration).onComplete = () => Destroy(activePlayer.gameObject);
        }

        if (Math.Abs(_Duration) > Mathf.Epsilon)
            Content.DOAnchorPos(Vector3.up * Screen.height, LevelSelectorComponent.UiAnimationDuration);
        else
            Content.position += Vector3.up * Screen.height;

        IsFocused = false;
    }

    void ChoosePlayer(float _Duration)
    {
        if (Math.Abs(_Duration) > Mathf.Epsilon)
            Content.DOAnchorPos(Vector3.up * Screen.height, LevelSelectorComponent.UiAnimationDuration);
        else
            Content.position += Vector3.up * Screen.height;

        IsFocused = false;
    }

    void OnChoose()
    {
        Account.CollectionDefaultItemId = Index;
        ChoosePlayer(LevelSelectorComponent.UiAnimationDuration);
        LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs(activePlayer));
    }

    void OnBack()
    {
        Back();
    }

    void Back()
    {
        HideCollection(LevelSelectorComponent.UiAnimationDuration);
        LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs(null));
    }
    
    protected override void MoveLeft()
    {
        if (Index == 0 || !LeftBtn.gameObject.activeInHierarchy || !LeftBtn.Interactable || !MobileInput.Condition)
        {
            Debug.Log("Selection's already on the first element or left button disabled");
            return;
        }
        
        Index--;
        DisplayItem(Index, 1);
    }

    protected override void MoveRight()
    {
        if (Index == Length - 1 || !RightBtn.gameObject.activeInHierarchy || !RightBtn.Interactable || !MobileInput.Condition)
        {
            Debug.Log("Selection's already on the last element or right button disabled");
            return;
        }
        
        Index++;
        DisplayItem(Index, -1);
    }

//]    protected override void OnSwipeUp()
//    {
////        base.OnSwipeUp();
//        Back();
//    }

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

    protected override void ProcessOffset()
    {
//        throw new NotImplementedException();
    }

    void ShowCollectionEvent_Handler(ShowCollectionEventArgs _Args)
    {
        ShowCollection();
    }
}
