using Puzzle;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class InfinityTimelineListener : TimelineListener
{
    [Space(8), SerializeField, Tooltip("The Scriptable object with items")]
    private PlayableAsset[] _playableAssets;

    private int _indexAsset = 0;

    protected override void Start()
    {
        base.Start();
        _playableDirector.playableAsset = _playableAssets[Random.Range(0, 2)];
    }

    public override void OnNotify(Playable origin, INotification notification, object context)
    {
        if (GameSceneManager.Instance is InfinityGameSceneManager instance)
        {
            switch (notification)
            {
                case ChangeDifficultyMarker difficultyMarker:
                    instance.InvokeChangeDifficultyInfinitySpawner(difficultyMarker.difficultyParams.difficulty);
                    break;
                case LevelEndMarker _:
                    Debug.Log("Notification type: level end marker");
                    nextIndex();
                    changeTimeLine();
                    instance.InvokeChangeLevel();
                    return;
            }
        }
    }

    private void OnEnable()
    {
        base.OnEnable();
    }

    private void OnDisable()
    {
        base.OnDisable();
    }

    private void changeTimeLine()
    {
        _playableDirector.Stop();
        _playableDirector.time = 0;
        _playableDirector.playableAsset = _playableAssets[_indexAsset];
        _playableDirector.Play();
    }

    private void nextIndex()
    {
        if (_indexAsset + 1 >= _playableAssets.Length)
            _indexAsset = 0;
        else
            _indexAsset++;
    }
}