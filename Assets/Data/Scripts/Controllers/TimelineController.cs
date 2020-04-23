using Puzzle;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public abstract class TimelineController : MonoBehaviour
{
    [Space, SerializeField, Tooltip("The Scriptable objects with items")]
    protected PlayableAsset[] playableAssets;

    protected int _indexAsset = 0;

    protected PlayableDirector _playableDirector;

    private void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        StartIndex();
        _playableDirector.playableAsset = playableAssets[_indexAsset];
        _playableDirector.Play();
    }

    protected virtual void StartIndex()
    {
        _indexAsset = 0;
    }

    protected void NextTimeLine()
    {
        NextIndex();
        _playableDirector.playableAsset = playableAssets[_indexAsset];
        _playableDirector.Play(); 
        _playableDirector.time = 0;
    }

    protected void NextIndex()
    {
        if (_indexAsset + 1 >= playableAssets.Length)
            _indexAsset = 0;
        else
            _indexAsset++;
    }

    protected void RandomIndex()
    {
        _indexAsset = Random.Range(0, playableAssets.Length);
    }
}
