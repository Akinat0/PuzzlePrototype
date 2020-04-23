using Puzzle;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayableDirector))]
public class InfinitySoundTimelineListener : TimelineListener
{

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
                    instance.InvokeChangeTimeLine();
                    return;
            }
        }
    }
}
