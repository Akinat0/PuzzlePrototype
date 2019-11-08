using UnityEngine;

namespace Puzzle{
public class GameSceneManager : MonoBehaviour
{

    public static GameSceneManager Instance; 
    
    [SerializeField] public Player player;
    [SerializeField] public HealthManager healthManager;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
    }
}
}