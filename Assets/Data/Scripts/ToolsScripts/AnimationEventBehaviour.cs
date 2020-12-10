using System;
using UnityEngine;

public class AnimationEventBehaviour : StateMachineBehaviour
{
    [SerializeField] private string stateID;

    private bool completed = false;

    public string StateId => stateID;

    public event Action<string> OnStateExitEvent;
    public event Action<string> OnStateCompleteEvent;
    
    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 1)
        {
            if(StateId == "Show")
                Debug.Log("");
            
            if(!completed)
                OnStateCompleteEvent?.Invoke(StateId);
            completed = true;
        }
        else
            completed = false;
        
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStateExitEvent?.Invoke(StateId);
    }
    
}
