using System;
using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
public class StarView : MonoBehaviour
{
    bool isShown;
    bool isActive;

    SpriteRenderer background;

    public SpriteRenderer Background
    {
        get
        {
            if (background == null)
                background = GetComponent<SpriteRenderer>();
            
            return background;
        }
    }
    
    Animator Animator;

    static readonly int ShowID = Animator.StringToHash("Show");
    static readonly int InstantID = Animator.StringToHash("Instant");
    static readonly int ActiveID = Animator.StringToHash("Active");

    Action HideStateAction;
    Action ShowStateAction;
    
    AnimationEventBehaviour HideState;
    AnimationEventBehaviour ShowState;

    void Awake()
    {
        Animator = GetComponent<Animator>();
        HideState = AnimationEventBehaviour.FindState(Animator, "Hide");
        ShowState = AnimationEventBehaviour.FindState(Animator, "Show");
        
        HideState.OnExit += InvokeHideState;
        ShowState.OnExit += InvokeShowState;
    }

    void OnEnable()
    {
        if (isShown)
        {
            Animator.SetBool(ShowID, true);
            Animator.SetBool(InstantID, true);
            Animator.SetBool(ActiveID, isActive);
        }
    }

    public void Show(bool active, bool instant = false, Action finished = null)
    {
        isShown = true;
        isActive = active;
        
        ShowStateAction += finished;
        Animator.SetBool(InstantID, instant);
        Animator.SetBool(ShowID, true);
        Animator.SetBool(ActiveID, active);
    }

    public void Hide(Action finished = null)
    {
        isShown = false;
        
        HideStateAction += finished;
        Animator.SetBool(ShowID, false);
    }

    void InvokeHideState()
    {
        Action action = HideStateAction;
        HideStateAction = null;
        action?.Invoke();
    }
    
    void InvokeShowState()
    {
        Action action = ShowStateAction;
        ShowStateAction = null;
        action?.Invoke();
    }
}
