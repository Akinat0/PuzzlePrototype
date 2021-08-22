using System;
using UnityEngine;

public abstract class TutorialHole
{
    public event Action RectChanged;
    
    public abstract Rect GetWorldRect();

    protected void InvokeRectChanged()
    {
        RectChanged?.Invoke();
    }
}

public class RectTutorialHole : TutorialHole
{
    public RectTutorialHole(Rect worldRect)
    {
        WorldRect = worldRect;
    }

    public Rect WorldRect { get; private set; }

    public void SetWorldRect(Rect worldRect)
    {
        if(worldRect == WorldRect)
            return;

        WorldRect = worldRect;
        InvokeRectChanged();
    }

    public override Rect GetWorldRect() => WorldRect;
    
}

public class RectTransformTutorialHole : TutorialHole
{
    public RectTransformTutorialHole(RectTransform rectTransform)
    {
        RectTransform = rectTransform;
        WorldRect = RectTransform.TransformRect(RectTransform.rect);
    }
    
    RectTransform RectTransform { get; }
    Rect WorldRect { get; set; }
    
    public override Rect GetWorldRect() => WorldRect;

    public void UpdateRect()
    {
        WorldRect = RectTransform.TransformRect(RectTransform.rect);
        InvokeRectChanged();
    }
}