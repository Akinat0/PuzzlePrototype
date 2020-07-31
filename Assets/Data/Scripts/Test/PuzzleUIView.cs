using UnityEngine;
using UnityEngine.UI;

public class PuzzleUIView : MonoBehaviour
{
    [SerializeField] SetPuzzlesForCamera puzzlesForCamera;
    [SerializeField] CollectionItem collectionItem;

    RawImage image;

    bool IsVisible = false;

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
    
    void Start()
    {
        image = GetComponent<RawImage>();
        image.uvRect = puzzlesForCamera.GetPuzzleRectInAtlas(collectionItem.ID);

        puzzlesForCamera.RebuildPuzzlesAtlas += OnRebuildPuzzlesAtlas_Handler;
    }

    void OnDisable()
    {
        puzzlesForCamera.DeactivateItem(collectionItem.ID);
        IsVisible = false;
    }

    void OnRebuildPuzzlesAtlas_Handler()
    {
        if (gameObject.activeInHierarchy && IsVisible)
            image.uvRect = puzzlesForCamera.GetPuzzleRectInAtlas(collectionItem.ID);
    }

    void Update()
    {
        bool isRectVisible = RectTransform.IsVisibleOnTheScreen();
        
//        Debug.LogError(isRectVisible);
        
        if (isRectVisible && !IsVisible)
        {
            IsVisible = true;
            image.uvRect = puzzlesForCamera.GetPuzzleRectInAtlas(collectionItem.ID);
        }
        else
        {
            if (!isRectVisible && IsVisible)
            {
                IsVisible = false;
                puzzlesForCamera.DeactivateItem(collectionItem.ID);
            }
        }
        
    }
}
