using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIPuzzleView : UIComponent
{
    [SerializeField] CollectionItem collectionItem;

    protected bool IsVisible;
    
    RawImage puzzleImage;
    public virtual RawImage PuzzleImage
    {
        get
        {
            if (puzzleImage == null)
                puzzleImage = GetComponent<RawImage>();
            
            return puzzleImage;
        }
    }
    
    RectTransform rectTransform;
    public virtual RectTransform RectTransform
    {
        get
        {
            if(rectTransform == null)
                rectTransform = transform as RectTransform;
            return rectTransform;
        }
    }
    
    protected virtual void Start()
    {
        puzzleImage = GetComponent<RawImage>();
        puzzleImage.uvRect = RuntimePuzzleAtlas.Instance.GetPuzzleRectInAtlas(collectionItem.ID);

        RuntimePuzzleAtlas.Instance.RebuildPuzzlesAtlas += OnRebuildPuzzlesAtlas_Handler;
    }
    protected void Update()
    {
        bool isRectVisible = RectTransform.IsVisibleOnTheScreen();

        if (isRectVisible && !IsVisible)
        {
            IsVisible = true;
            puzzleImage.uvRect = RuntimePuzzleAtlas.Instance.GetPuzzleRectInAtlas(collectionItem.ID);
        }
        else
        {
            if (!isRectVisible && IsVisible)
            {
                IsVisible = false;
                RuntimePuzzleAtlas.Instance.DeactivateItem(collectionItem.ID);
            }
        }
        
    }
    protected void OnDisable()
    {
        RuntimePuzzleAtlas.Instance.DeactivateItem(collectionItem.ID);
        IsVisible = false;
    }

    void OnRebuildPuzzlesAtlas_Handler()
    {
        if (gameObject.activeInHierarchy && IsVisible)
            puzzleImage.uvRect = RuntimePuzzleAtlas.Instance.GetPuzzleRectInAtlas(collectionItem.ID);
    }

}
