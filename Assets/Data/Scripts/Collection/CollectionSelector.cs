using Abu.Tools;
using DG.Tweening;
using ScreensScripts;
using UnityEngine;
using UnityEngine.UI;

public class CollectionSelector : SelectorBase
{
    [SerializeField] private CollectionItem[] Selection;
    [SerializeField] private Transform ItemContainer;
    [SerializeField] private RectTransform Content;
    protected override int Length => Selection.Length;
    
    protected override void Start()
    {
        base.Start();
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
        activePlayer.transform.DOMove(Vector3.zero, SelectionManager.UiAnimationDuration);
        Content.DOAnchorPos(Vector2.zero, SelectionManager.UiAnimationDuration);
    }
    
    private void HideCollection()
    {
        Content.localPosition += Vector3.up * Content.rect.size.y;
        Content.gameObject.SetActive(false);
    }

    public void OnChoose()
    {
        HideCollection();
        
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
