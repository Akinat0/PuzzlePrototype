using UnityEngine;

namespace Puzzle{
public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance; 
    
    [SerializeField] private Animator gameCameraAnimator;
    [SerializeField] public Player player;
    [SerializeField] public HealthManager healthManager;
    [SerializeField] public SoundManager soundManager;
    [SerializeField] public Score score;

    private static readonly int Shake = Animator.StringToHash("shake");

    void Awake()
    {
        Instance = this;
    }

    public void ShakeCamera()
    { 
        gameCameraAnimator.SetTrigger(Shake);
    }
}
}