using System;
using System.Linq;
using UnityEngine;

public class AnimationEventBehaviour : StateMachineBehaviour
{
    [SerializeField] private string stateID;

    public event Action OnEnter;
    public event Action OnExit;

    public event Action<string> OnExitState;
    public event Action OnComplete;

    bool Completed;
    
    public override void OnStateEnter(Animator _Animator, AnimatorStateInfo _StateInfo, int _LayerIndex)
    {
        Completed = false;
        OnEnter?.Invoke();
    }

    public override void OnStateUpdate(Animator _Animator, AnimatorStateInfo _StateInfo, int _LayerIndex)
    {
        if (!Completed && OnComplete != null && _StateInfo.normalizedTime >= 1)
        {
            Completed = true;
            OnComplete();
        }
    }

    public override void OnStateExit(Animator _Animator, AnimatorStateInfo _StateInfo, int _LayerIndex)
    {
        if (!Completed)
        {
            Completed = true;
            OnComplete?.Invoke();
        }
        
        OnExit?.Invoke();
        OnExitState?.Invoke(stateID);
    }

    public static AnimationEventBehaviour FindState(Animator animator, string stateId)
    {
        return animator.GetBehaviours<AnimationEventBehaviour>()
            .FirstOrDefault(state => state.stateID == stateId);
    }

}
