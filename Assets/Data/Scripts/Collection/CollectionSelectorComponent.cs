using System;
using Abu.Tools;
using Abu.Tools.UI;
using DG.Tweening;
using ScreensScripts;
using UnityEngine;

public class CollectionSelectorComponent : SelectorComponent<CollectionItem>
{
    [SerializeField] TextButtonComponent InteractBtn;
    [SerializeField] ButtonComponent HomeBtn;
    [SerializeField] Transform ItemContainer;
    [SerializeField] RectTransform Content;

    protected override CollectionItem[] Selection { get; set; }
    protected override int Index { get; set; }
    protected override int Length => Selection.Length;

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
        //Managing right button
        RightBtn.SetActive(_Index + 1 != Length);

        //Managing left button
        LeftBtn.SetActive(_Index != 0);
            
        CreateItem(_Index);
            
        if (Length == 0)
        {
            RightBtn.SetActive(false);
            LeftBtn.SetActive(false);
        }

        Vector3 shift = _Direction * ScreenScaler.CameraSize.x * Vector3.right;
        
        activePlayer.transform.position = - shift;

        if(_Direction != 0)
        {
            oldPlayer.transform.DOMove(shift,
                    SelectionManager.PlayerAnimationDuration)
                .OnStart(() =>
                {
                    RightBtn.Interactable = false;
                    LeftBtn.Interactable = false;
                })
                .OnComplete(() =>
                {
                    RightBtn.Interactable = true;
                    LeftBtn.Interactable = true;
                    Destroy(oldPlayer.gameObject);
                });
            
            activePlayer.transform.DOMove(Vector3.zero, 
                SelectionManager.PlayerAnimationDuration);
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

    void ShowCollection()
    {
        Content.gameObject.SetActive(true);
        DisplayItem(Index);

        activePlayer.transform.localPosition += Vector3.up * ScreenScaler.CameraSize.y;
        
        activePlayer.transform.DOMove(Vector3.zero, SelectionManager.UiAnimationDuration)
            .SetDelay(SelectionManager.UiAnimationDuration / 2);
        
        Content.DOAnchorPos(Vector2.zero, SelectionManager.UiAnimationDuration)
            .SetDelay(SelectionManager.UiAnimationDuration / 2);
    }
    
    void HideCollection(float _Duration = 0)
    {
        if (activePlayer != null)
        {
            activePlayer.transform.DOMove(Vector3.up * ScreenScaler.CameraSize.y,
                SelectionManager.UiAnimationDuration).onComplete = () => Destroy(activePlayer.gameObject);
        }

        if (Math.Abs(_Duration) > Mathf.Epsilon)
            Content.DOAnchorPos(Vector3.up * Screen.height, SelectionManager.UiAnimationDuration);
        else
            Content.position += Vector3.up * Screen.height;
        
    }

    void ChoosePlayer(float _Duration)
    {
        if (Math.Abs(_Duration) > Mathf.Epsilon)
            Content.DOAnchorPos(Vector3.up * Screen.height, SelectionManager.UiAnimationDuration);
        else
            Content.position += Vector3.up * Screen.height;
    }

    void OnChoose()
    {
        Account.CollectionDefaultItemId = Index;
        ChoosePlayer(SelectionManager.UiAnimationDuration);
        LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs(activePlayer));
    }

    void OnBack()
    {
        HideCollection(SelectionManager.UiAnimationDuration);
        LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs(null));
    }
    
    protected override void MoveLeft()
    {
        Index--;
        DisplayItem(Index, 1);
    }

    protected override void MoveRight()
    {
        Index++;
        DisplayItem(Index, -1);
    }

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
    
    void ShowCollectionEvent_Handler(ShowCollectionEventArgs _Args)
    {
        ShowCollection();
    }
}
