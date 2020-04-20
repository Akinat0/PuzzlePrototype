using Puzzle;
using UnityEngine;

public class TutorialEnemyComponent : MonoBehaviour
{
    private void OnEnable()
    {
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
    }
    
    private void OnDisable()
    {
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
    }

    void PlayerLosedHpEvent_Handler(int hp)
    {
        Destroy(gameObject);
    }
}
