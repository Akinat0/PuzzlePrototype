using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Animator))]
public class StarView : MonoBehaviour
{

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

    private static readonly int ShowID = Animator.StringToHash("Show");
    private static readonly int InstantID = Animator.StringToHash("Instant");
    private static readonly int ActiveID = Animator.StringToHash("Active");

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
    
    
    public void Show(bool active, bool instant = false, Action finished = null)
    {
        ShowStateAction += finished;
        Animator.SetBool(InstantID, instant);
        Animator.SetBool(ShowID, true);
        Animator.SetBool(ActiveID, active);
    }

    public void Hide(Action finished = null)
    {
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
