using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HeartView : MonoBehaviour
{
    [SerializeField] private GameObject vfx;
    
    private Animator animator;
    private static readonly int Visible = Animator.StringToHash("Visible");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Disappear(bool withVfx = true)
    {
        animator.SetBool(Visible, false);
        if(withVfx)
            Instantiate(vfx, transform);
    }
    public void Appear(bool withVfx = true)
    {
        animator.SetBool(Visible, true);
        if(withVfx)
            Instantiate(vfx, transform);
    }
}
