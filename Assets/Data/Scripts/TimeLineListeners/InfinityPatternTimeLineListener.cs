using Puzzle;
using PuzzleScripts;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class InfinityPatternTimeLineListener : TimelineListener
{
    private float _timeLineSpeed;
    private float _enemySpeed;
    private float _futureTimeLineSpeed;
    private float _futureEnemySpeed;
    private bool _reverseStickOut;
    private bool _reverseSide;
    private EnemyParams _enemyParams;

    public new void Start()
    {
        if (GameSceneManager.Instance is InfinityGameSceneManager instance)
        {
            _timeLineSpeed = instance.startPatternTimeLineSpeed;
            _futureTimeLineSpeed = instance.startPatternTimeLineSpeed;

            _enemySpeed = instance.startEnemySpeed;
            _futureEnemySpeed = instance.startEnemySpeed;
        }

        _reverseStickOut = false;
        _reverseSide = false;
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
                    SetNewReverses();
                    return;
            }
        }
    }

    private EnemyParams SetEnemyParams(EnemyParams enemyParams)
    {
        enemyParams.speed = _enemySpeed;
        enemyParams.stickOut = _reverseStickOut ? enemyParams.stickOut : !enemyParams.stickOut;
        enemyParams.side = _reverseSide ? ReverseSide(enemyParams.side) : enemyParams.side;
        return enemyParams;
    }

    private void SetCurrentSpeeds()
    {
        _enemySpeed = _futureEnemySpeed;
        _timeLineSpeed = _futureTimeLineSpeed;
        _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(_timeLineSpeed);
    }

    private void SetNewReverses()
    {
        _reverseStickOut = Random.value > 0.5;
        _reverseSide = Random.value > 0.5;
    }

    private Side ReverseSide(Side side)
    {
        switch (side)
        {
            case Side.Left:
                return Side.Right;
            case Side.Right:
                return Side.Left;
            case Side.Up:
                return Side.Down;
            case Side.Down:
                return Side.Up;
        }
        return side;
    }

    private new void OnEnable()
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
        _futureTimeLineSpeed = speed;
    }

    private void ChangeEnemySpeed_Handler(float speed)
    {
        _futureEnemySpeed = speed;
    }
}
