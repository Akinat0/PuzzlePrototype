using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HeartView : MonoBehaviour
{
    private Animator animator;
    private static readonly int Visible = Animator.StringToHash("Visible");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Disappear()
    {
        animator.SetBool(Visible, false);
    }
    public void Appear()
    {
        animator.SetBool(Visible, true);
    }
}
