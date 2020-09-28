﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleLayoutGroup : LayoutGroup
{

    public enum FitType
    {
        Uniform,
        Width, 
        Height,
        FixedRows,
        FixedColumns
    }

    public FitType fitType;
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    public bool fitX;
    public bool fitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        
        if (fitType == FitType.Height || fitType == FitType.Width || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;
            
            float sqrRt = Mathf.Sqrt(transform.childCount);

            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
            
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float) columns);
        }

        if (fitType == FitType.Height ||  fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float) rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;
        
        float cellWidth = parentWidth / columns - spacing.x / columns * columns -1 - (padding.left + padding.right) / (float)columns;
        float cellHeight = parentHeight / rows - spacing.y / rows * rows -1 - (padding.top + padding.bottom) / (float)rows;;

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            RectTransform item = rectChildren[i];

            float xPos = cellSize.x * columnCount + (spacing.x * columnCount) + padding.left;
            float yPos = cellSize.y * rowCount + (spacing.y * rowCount) + padding.top;
            
            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }    
    }

    public override void CalculateLayoutInputVertical()
    {
        
    }


    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}
