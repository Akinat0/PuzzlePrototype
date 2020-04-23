using Puzzle;
using PuzzleScripts;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class InfinityPatternTimeLineListener : TimelineListener
{
    private float _timeLineSpeed;
    private float _enemySpeed;
    private float _prevTimeLineSpeed;
    private float _prevEnemySpeed;
    private bool _reverseStickOut;
    private EnemyParams _enemyParams;

    protected override void Start()
    {
        _timeLineSpeed = 1;
        _enemySpeed = 4;
        _prevTimeLineSpeed = 1;
        _prevEnemySpeed = 4;
        _reverseStickOut = false;
        base.Start();
    }

    public override void OnNotify(Playable origin, INotification notification, object context)
    {
        if (GameSceneManager.Instance is InfinityGameSceneManager instance)
        {
            switch (notification)
            {
                case EnemyNotificationMarker enemyMarker:
                    _enemyParams = SetEnemyParams(enemyMarker.enemyParams);
                    instance.InvokeCreateEnemy(_enemyParams);
                    break;
                case LevelEndMarker _:
                    instance.InvokeChangePattern();
                    SetCurrentSpeeds();
                    SetNewReverseStickOut();
                    return;
            }
        }
    }

    private EnemyParams SetEnemyParams(EnemyParams enemyParams)
    {
        enemyParams.speed = _enemySpeed;
        enemyParams.stickOut = _reverseStickOut ? enemyParams.stickOut : !enemyParams.stickOut;
        return enemyParams;
    }

    private void SetCurrentSpeeds()
    {
        _enemySpeed = _prevEnemySpeed;
        _timeLineSpeed = _prevTimeLineSpeed;
        _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(_timeLineSpeed);
    }

    private void SetNewReverseStickOut()
    {
        _reverseStickOut = Random.value > 0.5;
    }

    protected override void OnEnable()
    {
        InfinityGameSceneManager.ChangePatternTimeLineSpeedEvent += ChangePatternTimeLineSpeed_Handler;
        InfinityGameSceneManager.ChangeEnemySpeedEvent += ChangeEnemySpeed_Handler;
        base.OnEnable();
    }

    private new void OnDisable()
    {
        InfinityGameSceneManager.ChangePatternTimeLineSpeedEvent -= ChangePatternTimeLineSpeed_Handler;
        InfinityGameSceneManager.ChangeEnemySpeedEvent -= ChangeEnemySpeed_Handler;
        base.OnDisable();
    }

    private void ChangePatternTimeLineSpeed_Handler(float speed)
    {
        _prevTimeLineSpeed = speed;
    }

    private void ChangeEnemySpeed_Handler(float speed)
    {
        _prevEnemySpeed = speed;
    }
}
