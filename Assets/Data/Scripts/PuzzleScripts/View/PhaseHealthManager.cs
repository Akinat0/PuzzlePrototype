using Puzzle;
using UnityEngine;

public class PhaseHealthManager : HealthManagerBase
{
    private DamagableView _damagableView;
    private void Start()
    {
        _damagableView = GameSceneManager.Instance.GetPlayer().GetComponent<DamagableView>();
        
        if (_damagableView == null)
            Debug.LogWarning("Can't find DamagableView on the player");
    }

    protected override void LoseHeart(int _Hp)
    {
        throw new System.NotImplementedException();
    }

    protected override void Reset()
    {
        throw new System.NotImplementedException();
    }
}
