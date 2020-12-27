using Puzzle;
using UnityEngine;

public class RitualMoonHaloComponent : MonoBehaviour
{
    void OnEnable()
    {
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }
    
    void OnDisable()
    {
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
    }
    
    void ResetLevelEvent_Handler()
    {
        gameObject.SetActive(false);
    }
    
}
