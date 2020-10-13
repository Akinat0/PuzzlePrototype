using Puzzle;
using UnityEngine;

public class LongPuzzleClaimedFX : MonoBehaviour
{
    void Awake()
    {
        if(VFXManager.Instance != null && GameSceneManager.Instance != null)
            VFXManager.Instance.CallTapRippleEffect(GameSceneManager.Instance.Player.transform.position);        
    }
}
