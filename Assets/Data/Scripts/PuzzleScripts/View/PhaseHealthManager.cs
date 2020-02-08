using Puzzle;
using UnityEngine;

public class PhaseHealthManager : HealthManagerBase
{
    private DamagableView _damagableView;
    
    protected override void LoseHeart(int _Hp)
    {
        if (_damagableView != null)
            _damagableView.SetSkinsPhase(_Hp - 1);
    }

    protected override void Reset()
    {
        if (_damagableView != null)
            _damagableView.SetSkinsPhase(Player.DEFAULT_HP - 1);    
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameSceneManager.GameStartedEvent += GameStartedEvent_Handler;
    }
    protected override void OnDisable()
    {
        base.OnEnable();
        GameSceneManager.GameStartedEvent -= GameStartedEvent_Handler;
    }

    void GameStartedEvent_Handler()
    {
        _damagableView = GameSceneManager.Instance.GetPlayer()
            .GetComponent<DamagableView>();
        
        if (_damagableView == null)
            Debug.LogError("Can't find DamagableView on the player");
    }
}
