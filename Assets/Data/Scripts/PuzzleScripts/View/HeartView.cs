using Abu.Tools;
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
        Background.sortingLayerName = RenderLayer.VFX;
    }

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
    
    public Vector2 Size => ScreenScaler.SpriteRectInWorld(Background).size;

    public void Hide(bool withVfx = true)
    {
        animator.SetBool(Visible, false);
        if(withVfx)
            Instantiate(vfx, transform);
    }
    public void Show(bool withVfx = true)
    {
        animator.SetBool(Visible, true);
        if(withVfx)
            Instantiate(vfx, transform);
    }
}
