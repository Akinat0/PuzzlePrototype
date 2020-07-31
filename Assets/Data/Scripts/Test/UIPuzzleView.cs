using Abu.Tools.UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIPuzzleView : UIComponent
{

    #region factory

    static UIPuzzleView prefab;

    static UIPuzzleView Prefab
    {
        get
        {
            if(prefab == null)
                prefab = Resources.Load<UIPuzzleView>("UI/UIPuzzleView");
            
            return prefab;
        }
    }
    
    public static UIPuzzleView Create(int id, RectTransform parent)
    {
        UIPuzzleView puzzleView = Instantiate(Prefab, parent); 
        
        puzzleView.ID = id;
        puzzleView.FitIntoParent();
        
        return puzzleView;
    }

    #endregion

    int id;
    public int ID
    {
        get => id;
        set { id = value; } //TODO rebuild if id changes
    }

    protected bool IsVisible { get; set; }

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

    public void FitIntoParent()
    {
        RectTransform parent = RectTransform.parent as RectTransform;
        
        if(parent == null)
            return;

        float targetEdgeSize = parent.rect.width > parent.rect.height ? parent.rect.height : parent.rect.width;
        
        float sizeDelta = RectTransform.rect.width - targetEdgeSize;
        
        RectTransform.sizeDelta = new Vector2(sizeDelta, sizeDelta);
    }
    
    protected virtual void Start()
    {
        PuzzleImage.uvRect = RuntimePuzzleAtlas.Instance.GetPuzzleRectInAtlas(ID);

        RuntimePuzzleAtlas.Instance.RebuildPuzzlesAtlas += OnRebuildPuzzlesAtlas_Handler;
    }
    protected void Update()
    {
        bool isRectVisible = RectTransform.IsVisibleOnTheScreen();

        if (isRectVisible && !IsVisible)
        {
            IsVisible = true;
            PuzzleImage.uvRect = RuntimePuzzleAtlas.Instance.GetPuzzleRectInAtlas(ID);
        }
        else
        {
            if (!isRectVisible && IsVisible)
            {
                IsVisible = false;
                RuntimePuzzleAtlas.Instance.DeactivateItem(ID);
            }
        }
        
    }
    protected void OnDisable()
    {
        RuntimePuzzleAtlas.Instance.DeactivateItem(ID);
        IsVisible = false;
    }

    void OnRebuildPuzzlesAtlas_Handler()
    {
        if (gameObject.activeInHierarchy && IsVisible)
            PuzzleImage.uvRect = RuntimePuzzleAtlas.Instance.GetPuzzleRectInAtlas(ID);
    }

}
