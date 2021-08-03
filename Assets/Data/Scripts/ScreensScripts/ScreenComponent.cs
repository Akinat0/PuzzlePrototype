using System;
using UnityEngine;
using Abu.Tools.UI;

public class ScreenComponent : UIComponent
{
    [SerializeField] OverlayView overlay;

    public event Action OnOverlayClick;
    
    public bool Shown { get; private set; }

    public virtual void CreateContent()
    {
        
    }

    protected virtual void OnEnable() { }
    
    protected virtual void OnDisable() { }

    public virtual bool Show(Action finished = null)
    {
        if (Shown)
        {
            finished?.Invoke();
            return false;
        }

        Shown = true;

        if (overlay != null)
        {
            overlay.ChangePhase(1, 0.5f, finished);
            overlay.OnClick += OnOverlayClick;
        }
        else
        {
            finished?.Invoke();
        }

        return true;
    }

    public virtual bool Hide(Action finished = null)
    {
        if (!Shown)
        {
            finished?.Invoke();
            return false;
        }

        Shown = false;

        if (overlay != null)
        {
            overlay.ChangePhase(0, 0.5f, finished);
            overlay.OnClick -= OnOverlayClick;
        }
        else
        {
            finished?.Invoke();
        }

        return true;
    }
}
