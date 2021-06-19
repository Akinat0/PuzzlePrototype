using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class RandomAnimatorParameter : MonoBehaviour
{
    [SerializeField] string ParameterName = "RandomParameter";
    [SerializeField] int ParameterRange = 3;

    AnimationEventBehaviour[] EventBehaviours;
    int PreviousParameterValue = -1;
    
    Animator Animator;
    void Awake()
    {
        Animator = GetComponent<Animator>();

        EventBehaviours = Animator.GetBehaviours<AnimationEventBehaviour>();

        if (EventBehaviours == null || EventBehaviours.Length == 0)
            return;

        foreach (AnimationEventBehaviour behaviour in EventBehaviours)
            behaviour.OnExit += RandomizeParameter;
    }

    void OnDestroy()
    {
        foreach (AnimationEventBehaviour behaviour in EventBehaviours)
            behaviour.OnExit -= RandomizeParameter;
    }

    void RandomizeParameter()
    {
        int newParameterValue = Random.Range(0, ParameterRange);

        if (newParameterValue == PreviousParameterValue)
            newParameterValue = (newParameterValue + 1) % ParameterRange;
        

        PreviousParameterValue = newParameterValue;
        
        Animator.SetInteger(ParameterName, newParameterValue);
    }
    
    
    #if UNITY_EDITOR

    public static void SetSettings(RandomAnimatorParameter animatorParameter, int range, string parameterName = "RandomParameter")
    {
        animatorParameter.ParameterRange = range;
        animatorParameter.ParameterName = parameterName;
    }
    
    #endif
}
