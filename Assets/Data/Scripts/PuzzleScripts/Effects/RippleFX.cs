using Puzzle;
using UnityEngine;

public class RippleFX : MonoBehaviour
{
    
    [SerializeField] float Lifetime = 1;
    
    [SerializeField] Color StartInnerColor = new Color(0.98f, 0.87f, 0.29f, 1);
    [SerializeField] Color StartOuterColor = new Color(1, 0.54f, 0, 1);
    [SerializeField] float StartThickness = 1;
    [SerializeField] float StartSize = 0;
    
    [SerializeField] Color EndInnerColor = new Color(0.98f, 0.87f, 0.29f, 1);
    [SerializeField] Color EndOuterColor = new Color(1, 0.54f, 0, 1);
    [SerializeField] float EndThickness = 0;
    [SerializeField] float EndSize = 2;
    
    void Start()
    {
        if(VFXManager.Instance != null)
            VFXManager.Instance.CallTapRippleEffect(transform.position, Lifetime, StartSize, EndSize, StartThickness,
                EndThickness, StartInnerColor, StartOuterColor, EndInnerColor, EndOuterColor);        
    }
}
