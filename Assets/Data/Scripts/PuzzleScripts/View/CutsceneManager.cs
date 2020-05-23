using DG.Tweening;
using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private Text tipText;
    [SerializeField] private SceneTransitionType transitionType;
    
    public const float SkipTime = 2;
    private float holdTime = 0;

    private void Start()
    {
        progressBar.fillAmount = 0;
    }

    void Update()
    {
        ProcessInput();
    }


    void ProcessInput()
    {
#if UNITY_EDITOR
        
        if (Input.anyKey)
        {
            if(Input.anyKeyDown)
                ProcessTouchBegan();
            
            ProcessHold();
        }
        else
        {
            holdTime = 0;
            ProcessTouchEnded();
        }

#else
        if (Input.touchCount > 0)
        {
            TouchPhase touchPhase = Input.touches[0].phase;
            
            if(touchPhase == TouchPhase.Began)
                ProcessTouchBegan();
                
            if(touchPhase == TouchPhase.Moved || touchPhase == TouchPhase.Stationary )
                ProcessHold();
        }
        else
        {
            ProcessTouchEnded();
            holdTime = 0;
        }
#endif
    }

    void ProcessTouchBegan()
    {
        tipText.DOKill();
        tipText.DOFade(1, 0.6f).SetUpdate(true);
        progressBar.DOKill();
        progressBar.DOFade(1, 0.6f).SetUpdate(true);
    }
    
    void ProcessTouchEnded()
    {
        tipText.DOKill();
        tipText.DOFade(0, 0.6f).SetUpdate(true);
        progressBar.DOKill();
        progressBar.DOFade(0, 0.6f).SetUpdate(true);
    }
    
    void ProcessHold()
    {
        holdTime += Time.unscaledDeltaTime;
        progressBar.fillAmount = Mathf.Clamp01(holdTime / SkipTime);
        
        if (holdTime >= SkipTime)
            GameSceneManager.Instance.InvokeCutsceneEnded(new CutsceneEventArgs(gameObject.scene.name, transitionType));
    }
}
