using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DetachedVFX : MonoBehaviour
{
    Animator Animator;
    static readonly int Show = Animator.StringToHash("Show");

    AnimationEventBehaviour hideState;
    
    public void Initialize(Transform follower, Transform parent)
    {
        transform.parent = parent;
        transform.position = follower.position;
        transform.rotation = follower.rotation;
        transform.localScale *= follower.lossyScale.x;

        Animator = GetComponent<Animator>();
        hideState = Animator.GetBehaviours<AnimationEventBehaviour>().FirstOrDefault(beh => beh.StateId == "Hide");
    }

    public void Play()
    {
        Animator.SetBool(Show, true);    
    }

    public void Hide()
    
    {
        Animator.SetBool(Show, false);
        
        if(hideState != null)
            hideState.OnStateExitEvent += _ => Destroy(gameObject);
    }
}
