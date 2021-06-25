using DG.Tweening;
using Promises;
using Puzzle;
using UnityEngine;

public class GameProgressView : MonoBehaviour
{
    public class FirstStarPositionsRequest : Deferred<float> { }

    public class LevelPhaseRequest : Deferred<float> { }

    [SerializeField] GameProgressStar firstStar;
    [SerializeField] GameProgressStar secondStar;
    [SerializeField] GameProgressStar thirdStar;
    
    [SerializeField] RectTransform fillRect;

    CanvasGroup canvasGroup;
    
    bool initialized;

    void OnEnable()
    {
        GameSceneManager.HeartsAmountChangedEvent += HeartsAmountChangedEvent_Handler;
        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        GameSceneManager.LevelStartedEvent += LevelStartedEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }

    void OnDisable()
    {
        GameSceneManager.HeartsAmountChangedEvent -= HeartsAmountChangedEvent_Handler;
        GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
        GameSceneManager.LevelStartedEvent -= LevelStartedEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        
        GameSceneManager.Instance.Requests.Add(new FirstStarPositionsRequest().OnSuccess(Initialize));
    }

    void Update()
    {
        if (!initialized || GameSceneManager.Instance.Requests.Has<LevelPhaseRequest>())
            return;
        
        GameSceneManager.Instance.Requests.Add(new LevelPhaseRequest().OnResolved(UpdatePhase));
    }

    void Initialize(float firstStarPhase)
    {
        if (initialized)
            return;
        
        initialized = true;

        canvasGroup.DOFade(1, 1.5f);

        ActivateStars(GameSceneManager.Instance.LevelConfig.StarsAmount);
        SetupThirdStar(GameSceneManager.Instance.CurrentHearts);

        Vector2 min = firstStar.RectTransform.anchorMin;
        Vector2 max = firstStar.RectTransform.anchorMax;

        max.x = firstStarPhase;
        min.x = firstStarPhase;

        firstStar.RectTransform.anchorMin = min;
        firstStar.RectTransform.anchorMax = max;
    }

    void UpdatePhase(float phase)
    {
        fillRect.anchorMax = new Vector2(phase, 1);
    }

    void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
    {
        initialized = false;
    }
    
    void HeartsAmountChangedEvent_Handler(int heartsAmount)
    {
        if (!initialized || thirdStar.IsActive)
            return;
        
        SetupThirdStar(heartsAmount);
    }

    void LevelStartedEvent_Handler()
    {
        GameSceneManager.Instance.LevelConfig.StarsAmountChanged += StarsAmountChanged;
    }
    
    void LevelClosedEvent_Handler()
    {
        GameSceneManager.Instance.LevelConfig.StarsAmountChanged -= StarsAmountChanged;
    }

    void ResetLevelEvent_Handler()
    {
        SetupThirdStar(GameSceneManager.Instance.CurrentHearts);
    }

    void StarsAmountChanged(int starsAmount)
    {
        if(!initialized)
            return;
        
        ActivateStars(starsAmount);
    }

    void SetupThirdStar(float currentHearts)
    {
        thirdStar.Fill = currentHearts.Remap(
            GameSceneManager.Instance.LevelConfig.ThirdStarThreshold - 1, GameSceneManager.Instance.TotalHearts,
            0, 1);
    }
    
    void ActivateStars(int starsAmount)
    {
        switch (starsAmount)
        {
            case 3:
                firstStar.IsActive = true;
                secondStar.IsActive = true;
                thirdStar.IsActive = true;
                break;
            case 2:
                firstStar.IsActive = true;
                secondStar.IsActive = true;
                break;
            case 1:
                firstStar.IsActive = true;
                break;
        }
    }
}
