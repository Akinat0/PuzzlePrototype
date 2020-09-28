using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class PuzzleGodMode : MonoBehaviour
{
    [SerializeField, Range(0, 5)] float SpeedUp = 2;

    PlayableDirector Director;
    
    void Awake()
    {
        Director = GetComponent<PlayableDirector>();
        
        if(Director == null)
            Debug.LogError("God Mode can be attached only on playable director");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            TimeManager.DefaultTimeScale = SpeedUp;
        else
            TimeManager.DefaultTimeScale = 1;
    }
}
