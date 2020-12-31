using Puzzle;
using UnityEngine;

public class RitualMoonHaloComponent : MonoBehaviour
{
    void OnEnable()
    {
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
    }
    
    void OnDisable()
    {
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
    }
    
    void LevelClosedEvent_Handler()
    {
        gameObject.SetActive(false);
    }
    
}
