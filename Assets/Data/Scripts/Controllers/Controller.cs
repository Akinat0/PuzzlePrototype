using Puzzle;
using UnityEngine;
using UnityEngine.Playables;

public class Controller : MonoBehaviour
{
    [Space, SerializeField, Tooltip("The Scriptable object with items")]
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

    protected void StartIndex()
    {
        _indexAsset = Random.Range(0, playableAssets.Length);
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
