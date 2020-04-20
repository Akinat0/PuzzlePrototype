using System;
using Abu.Tools;
using DG.Tweening;
using ScreensScripts;
using UnityEngine;
using UnityEngine.UI;

public class CollectionSelector : SelectorBase
{
    [SerializeField] private Transform LevelPlayerRoot;
    [SerializeField] private Transform ItemContainer;
    [SerializeField] private RectTransform Content;
    
    private CollectionItem[] Selection;
    

    protected override int Length => Selection.Length;
    
    protected override void Start()
    {
        ItemNumber = Account.CollectionDefaultItemId;
        Selection = Account.CollectionItems;
        HideCollection();
    }
    
    private PlayerView activePlayer;
    private PlayerView oldPlayer;
    protected override void CreateItem(int _Index)
    {
        oldPlayer = activePlayer;
        activePlayer = Instantiate(Selection[_Index].Item, ItemContainer).GetComponent<PlayerView>();
    }

    protected override void DisplayItem(int _Index, int _Direction = 0)
    {
        base.DisplayItem(_Index, _Direction);
        activePlayer.transform.position = - _Direction * ScreenScaler.CameraSize.x * Vector3.right;

        if(_Direction != 0)
        {
            oldPlayer.transform.DOMove(_Direction * ScreenScaler.CameraSize.x * Vector3.right,
                    SelectionManager.PlayerAnimationDuration)
                .OnStart(() =>
                {
                    RightArrow.GetComponent<Button>().interactable = false;
                    LeftArrow.GetComponent<Button>().interactable = false;
                })
                .OnComplete(() =>
                {
                    RightArrow.GetComponent<Button>().interactable = true;
                    LeftArrow.GetComponent<Button>().interactable = true;
                    Destroy(oldPlayer.gameObject);
                });
            
            activePlayer.transform.DOMove(Vector3.zero, 
                SelectionManager.PlayerAnimationDuration);
        }
    }

    private void ShowCollection()
    {
        Content.gameObject.SetActive(true);
        DisplayItem(ItemNumber);

        activePlayer.transform.localPosition += Vector3.up * ScreenScaler.CameraSize.y;
        activePlayer.transform.DOMove(Vector3.zero, SelectionManager.UiAnimationDuration)
            .SetDelay(SelectionManager.UiAnimationDuration / 2);
        Content.DOAnchorPos(Vector2.zero, SelectionManager.UiAnimationDuration)
            .SetDelay(SelectionManager.UiAnimationDuration / 2);
    }
    
    private void HideCollection(float _Duration = 0)
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

    private void ChoosePlayer(float _Duration)
    {
        if (Math.Abs(_Duration) > Mathf.Epsilon)
            Content.DOAnchorPos(Vector3.up * Screen.height, SelectionManager.UiAnimationDuration);
        else
            Content.position += Vector3.up * Screen.height;
    }

    public void OnChoose()
    {
        Account.CollectionDefaultItemId = ItemNumber;
        ChoosePlayer(SelectionManager.UiAnimationDuration);
        LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs(activePlayer));
    }

    public void OnBack()
    {
        HideCollection(SelectionManager.UiAnimationDuration);
        LauncherUI.Instance.InvokeCloseCollection(new CloseCollectionEventArgs(null));
    }
    
    void OnEnable()
    {
        LauncherUI.ShowCollectionEvent += ShowCollectionEvent_Handler;
    }
    
    void OnDisable()
    {
        LauncherUI.ShowCollectionEvent -= ShowCollectionEvent_Handler;
    }
    
    private void ShowCollectionEvent_Handler(ShowCollectionEventArgs _Args)
    {
        ShowCollection();
    }
}
