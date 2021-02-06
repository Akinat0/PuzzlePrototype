using System.Collections.Generic;
using System.Linq;
using Puzzle;
using UnityEngine;
using UnityEngine.Timeline;

public partial class TimelineListener
{
    void EditorStartLevel()
    {
        #if UNITY_EDITOR
        
        TimelineAsset timeLineAsset = (TimelineAsset) PlayableDirector.playableAsset;
        
        if(timeLineAsset == null)
            return;
        
        startTime = 0;
        List<PlayAudioMarker> audioMarkers = new List<PlayAudioMarker>();
        for (int i=0; i < timeLineAsset.markerTrack.GetMarkerCount(); i++)
        {
            IMarker marker = timeLineAsset.markerTrack.GetMarker(i);
            if (Mathf.Approximately((float)startTime, 0))
                if (marker is StartLevelMarker)
                    startTime = marker.time;
            if (marker is PlayAudioMarker playAudioMarker)
            {
                audioMarkers = audioMarkers.Append(playAudioMarker).ToList();
            }

            if (!Mathf.Approximately((float)startTime, 0))
            {
                PlayableDirector.time = startTime;
                foreach (var audioMarker in audioMarkers)
                {
                    if (audioMarker.time < startTime &&
                        audioMarker.time + audioMarker.AudioClip.length > startTime)
                    {
                        GameSceneManager.Instance.InvokePlayAudio(new LevelPlayAudioEventArgs(audioMarker.AudioClip,
                            audioMarker.Looped, audioMarker.SoundCurve, startTime-audioMarker.time));
                    }
                }
                break;
            }
        }
        
        #endif
    }
}
