using System;
using UnityEngine;
using Abu.Tools.UI;

public class ScreenComponent : UIComponent
{
    [SerializeField] OverlayView overlay;
    
    public bool Shown { get; private set; }

    public virtual void CreateContent()
    {
        
    }
    
    public virtual bool Show(Action finished = null)
    {
        if (Shown)
        {
            finished?.Invoke();
            return false;
        }

        Shown = true;
        
        if(overlay != null)
            overlay.ChangePhase(1, 0.5f, finished);
        else
            finished?.Invoke();

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
        
        overlay.ChangePhase(0, 0.5f, finished);
        
        return true;
    }
    
    
}
