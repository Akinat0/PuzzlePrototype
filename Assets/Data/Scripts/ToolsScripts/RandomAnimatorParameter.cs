using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class RandomAnimatorParameter : MonoBehaviour
{
    [SerializeField] private string ParameterName = "RandomParameter";
    [SerializeField] private int ParameterRange = 3;

    private AnimationEventBehaviour[] EventBehaviours;
    private int PreviousParameterValue = -1;
    
    private Animator Animator;
    private void Awake()
    {
        Animator = GetComponent<Animator>();

        EventBehaviours = Animator.GetBehaviours<AnimationEventBehaviour>();

        if (EventBehaviours == null || EventBehaviours.Length == 0)
            return;

        foreach (AnimationEventBehaviour behaviour in EventBehaviours)
            behaviour.OnExit += RandomizeParameter;
    }

    private void OnDestroy()
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
