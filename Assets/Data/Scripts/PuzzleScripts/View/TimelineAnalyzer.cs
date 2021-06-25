using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Promises;
using LevelPhaseRequest = GameProgressView.LevelPhaseRequest;
using FirstStarPositionsRequest = GameProgressView.FirstStarPositionsRequest; 

namespace Puzzle
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TimelineAnalyzer : MonoBehaviour
    {
        PlayableDirector playableDirector;

        PlayableDirector PlayableDirector =>
            playableDirector ? playableDirector : playableDirector = GetComponent<PlayableDirector>();

        TimelineAsset timeline;
        TimelineAsset Timeline => timeline ? timeline : timeline = (TimelineAsset)PlayableDirector.playableAsset;
        
        float? levelDuration;
        float? firstStarTime;

        bool levelCompleted;

        void OnEnable()
        {
            GameSceneManager.LevelStartedEvent += LevelStartedEvent_Handler;
            GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
            GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        }

        void OnDisable()
        {
            GameSceneManager.LevelStartedEvent -= LevelStartedEvent_Handler;
            GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
            GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
            GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
        }

        void Awake()
        {
            for (int i = 0; i < Timeline.markerTrack.GetMarkerCount(); i++)
            {
                IMarker marker = Timeline.markerTrack.GetMarker(i);
                
                switch (marker)
                {
                    case LevelEndMarker levelEndMarker:
                        levelDuration = (float) levelEndMarker.time;
                        break;
                    case FirstStarMarker firstStarMarker:
                        firstStarTime = (float) firstStarMarker.time;
                        break;
                }
            }
        }

        void LevelStartedEvent_Handler()
        {
            GameSceneManager.Instance.Requests.RequestAdded += RequestAdded_Handler;

            GameSceneManager.Instance.Requests.Get<LevelPhaseRequest>()?.Success(GetFirstStarPhase());
            GameSceneManager.Instance.Requests.Get<FirstStarPositionsRequest>()?.Success(GetLevelDuration());

            levelCompleted = false;
        }

        void LevelClosedEvent_Handler()
        {
            GameSceneManager.Instance.Requests.RequestAdded -= RequestAdded_Handler;
            
            levelCompleted = false;
        }
        
        void RequestAdded_Handler(Promise request)
        {
            switch (request)
            {
                case LevelPhaseRequest levelPhaseRequest:
                    levelPhaseRequest.Success(GetLevelDuration());
                    break;
                case FirstStarPositionsRequest starPositionsRequest:
                    starPositionsRequest.Success(GetFirstStarPhase());
                    break;
            }
        }

        void ResetLevelEvent_Handler()
        {
            levelCompleted = false;   
        }
        
        void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
        {
            levelCompleted = true;
        }

        float GetLevelDuration()
        {
            return levelCompleted ? 1 : levelDuration.HasValue ? (float) PlayableDirector.time / levelDuration.Value : 0;
        }

        float GetFirstStarPhase()
        {
            return levelDuration.HasValue && firstStarTime.HasValue ? firstStarTime.Value / levelDuration.Value : 1;
        }
    }
}