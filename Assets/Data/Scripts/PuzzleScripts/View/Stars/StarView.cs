using System;
using System.Linq;
using UnityEngine;

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
    private static readonly int HighlightID = Animator.StringToHash("Highlight");
    private static readonly int HideID = Animator.StringToHash("Hide");
    private static readonly int InstantID = Animator.StringToHash("Instant");
    private static readonly int ActiveID = Animator.StringToHash("Active");

    Action HideStateAction;
    Action ShowStateAction;
    
    AnimationEventBehaviour HideState;
    AnimationEventBehaviour ShowState;

    void Awake()
    {
        Animator = GetComponent<Animator>();
        HideState = Animator.GetBehaviours<AnimationEventBehaviour>().First(behaviour => behaviour.StateId == "Hide");
        ShowState = Animator.GetBehaviours<AnimationEventBehaviour>().First(behaviour => behaviour.StateId == "Show");

        HideState.OnStateExitEvent += InvokeHideState;
        ShowState.OnStateExitEvent += InvokeShowState;
    }
    
    public void Highlight()
    {
        Animator.SetTrigger(HighlightID);
    }
    
    public void Show(bool active, bool instant = false, Action finished = null)
    {
        ShowStateAction += finished;
        Animator.SetBool(InstantID, instant);
        Animator.SetTrigger(ShowID);
        Animator.SetBool(ActiveID, active);
    }

    public void Hide(Action finished = null)
    {
        HideStateAction += finished;
        Animator.SetTrigger(HideID);
    }

    void InvokeHideState(string stateID)
    {
        Action action = HideStateAction;
        HideStateAction = null;
        action?.Invoke();
    }
    
    void InvokeShowState(string stateID)
    {
        Action action = ShowStateAction;
        ShowStateAction = null;
        action?.Invoke();
    }
}
