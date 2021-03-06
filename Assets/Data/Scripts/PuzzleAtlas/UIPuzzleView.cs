﻿using Abu.Tools.UI;
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
        return puzzleView;
    }

    #endregion

    int id;
    public int ID
    {
        get => id;
        set => id = value; //TODO rebuild if id changes
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

    void OnDestroy()
    {
        RuntimePuzzleAtlas.Instance.RebuildPuzzlesAtlas -= OnRebuildPuzzlesAtlas_Handler;
    }

    void OnRebuildPuzzlesAtlas_Handler()
    {
        if (gameObject.activeInHierarchy && IsVisible)
            PuzzleImage.uvRect = RuntimePuzzleAtlas.Instance.GetPuzzleRectInAtlas(ID);
    }

}
