using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abu.Tools;
using Puzzle;
using UnityEngine;

public class HeartsHealthManager : MonoBehaviour
{
    [SerializeField] private HeartView HeartPrefab;
    [SerializeField] float HorizontalPadding = 0;
    [SerializeField] float HorizontalMargins = 0.05f;

    static readonly int AnimationReset = Animator.StringToHash("Reset");

    readonly List<HeartView> HeartViews = new List<HeartView>();

    protected int Hp => GameSceneManager.Instance.CurrentHearts;

    Animator heartsAnimator;
    Animator HeartsAnimator
    {
        get
        {
            if (heartsAnimator == null)
                heartsAnimator = GetComponent<Animator>();
            
            return heartsAnimator;
        }
    }

    void Setup(int heartsAmount)
    {
        if (heartsAmount > HeartViews.Count)
        {
            for (int i = HeartViews.Count; i < heartsAmount; i++)
            {
                HeartViews.Add(Instantiate(HeartPrefab, transform));
                HeartViews.Last().name = $"{HeartPrefab.name}_{i}";
            }
        }
        
        if (heartsAmount < HeartViews.Count)
        {
            for (int i = heartsAmount; i < HeartViews.Count; i++)
                Destroy(HeartViews[i].gameObject);
            
            HeartViews.RemoveRange(heartsAmount, HeartViews.Count - heartsAmount);
        }
        
        LayoutHearts();
        
        ProcessHearts(heartsAmount);
        
        ProcessHeartsAnimator();
    }

    void ProcessHeartsAnimator()
    {
        if (HeartsAnimator == null || !HeartsAnimator.isActiveAndEnabled)
            return;
        
        HeartsAnimator.SetTrigger(AnimationReset);
        HeartsAnimator.Rebind();
    }
    
    void ProcessHearts(int activeAmount)
    {
        if (activeAmount < 0 || activeAmount > HeartViews.Count)
        {
            Debug.LogWarning($"[HeartManager] Can't visualize Hp. Target amount {activeAmount}, hearts amount {HeartViews.Count}");
            return;
        }

        for (int i = 0; i < HeartViews.Count; i++)
        {
            bool shouldBeVisible = i < activeAmount;

            HeartView heart = HeartViews[i]; 
            
            if(heart.IsVisible && !shouldBeVisible)
                heart.Hide();
            
            if(!heart.IsVisible && shouldBeVisible)
                heart.Show();
        }
    }
    
    void LayoutHearts()
    {
        int length = HeartViews.Count;

        Vector2 camSize = ScreenScaler.CameraSize;
        
        float partOfScreen = 1.0f / length - HorizontalPadding - HorizontalMargins * 2 / length; 
        Vector3 targetScale = Vector3.one * ScreenScaler.FitHorizontalPart(HeartViews.First().Background, partOfScreen);

        for (int i = 0; i < length; i++)
        {
            HeartViews[i].transform.localScale = targetScale;
                
            Vector2 heartPosition = new Vector2
                (-camSize.x / 2 + HeartViews[i].Size.x / 2 + camSize.x / length * i,
                -camSize.y / 2 + camSize.y * 0.05f);

            HeartViews[i].transform.position = heartPosition;
        }

        float xOffset = (Mathf.Abs(HeartViews.Last().transform.position.x) - Mathf.Abs(HeartViews.First().transform.position.x)) / 2;

        foreach (Transform heartTransform in HeartViews.Select(heart => heart.transform))
            heartTransform.SetX(heartTransform.position.x - xOffset);
    }

    void StartAdditionalHeartAnimation()
    {
        HeartView additionalHeart = Instantiate(HeartPrefab, transform);
        additionalHeart.name = $"{HeartPrefab.name}_{HeartViews.Count}";
        HeartViews.Add(additionalHeart);

        additionalHeart.transform.position = new Vector3(0, -ScreenScaler.CameraSize.y / 4.0f, 0);
        additionalHeart.transform.localScale =
            Vector3.one * ScreenScaler.FitHorizontalPart(additionalHeart.Background, 0.4f);
        additionalHeart.Show();

        StartCoroutine(AdditionalHeartRoutine());
    }


    IEnumerator AdditionalHeartRoutine(float duration = 0.8f, Action finished = null)
    {
        Vector2 camSize = ScreenScaler.CameraSize;

        int length = HeartViews.Count;

        List<Vector3> sourceScales = new List<Vector3>();

        float partOfScreen = 1.0f / length - HorizontalPadding - HorizontalMargins * 2 / length;
        Vector3 targetScale = Vector3.one * ScreenScaler.FitHorizontalPart(HeartViews.First().Background,
                                  partOfScreen);

        List<Vector2> sourcePositions = new List<Vector2>();
        List<Vector2> targetPositions = new List<Vector2>();
        
        for (int i = 0; i < HeartViews.Count; i++)
        {
            Vector2 heartPosition = new Vector2
            (-camSize.x / 2 + HeartViews[i].Size.x / HeartViews[i].transform.localScale.x * targetScale.x / 2 + camSize.x / length * i,
                -camSize.y / 2 + camSize.y * 0.05f);

            targetPositions.Add(heartPosition);
            sourcePositions.Add(HeartViews[i].transform.position);
            sourceScales.Add(HeartViews[i].transform.localScale);
        }
        
        float xOffset = (Mathf.Abs(targetPositions.Last().x) - Mathf.Abs(targetPositions.First().x)) / 2;
        
        for (int i = 0; i < HeartViews.Count; i++)
            targetPositions[i] -= Vector2.right * xOffset;
        
        yield return new WaitForSeconds(1.2f);

        float time = 0;

        while (time <= duration)
        {
            for (int i = 0; i < HeartViews.Count; i++)
            {
                HeartViews[i].transform.position = Vector3.Lerp(sourcePositions[i], targetPositions[i], time / duration);
                HeartViews[i].transform.localScale = Vector3.Lerp(sourceScales[i], targetScale, time / duration);
            }

            time += Time.deltaTime;
            yield return null;
        }

        //Rebind animator keeping the same time and progress
        if (HeartsAnimator != null)
        {
            float animatorTime = HeartsAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            int stateHash = HeartsAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            HeartsAnimator.Rebind();
            HeartsAnimator.Update(0);
            HeartsAnimator.Play(stateHash, 0, animatorTime);
        }

        for (int i = 0; i < HeartViews.Count; i++)
        {
            HeartViews[i].transform.position = targetPositions[i];
            HeartViews[i].transform.localScale = targetScale;
        }
        
        finished?.Invoke();
    }
    
    #region event handlers
    
    protected virtual void OnEnable()
    {
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        GameSceneManager.PlayerReviveEvent += PlayerReviveEvent_Handler;
        GameSceneManager.HeartsAmountChangedEvent += HeartsAmountChangedEvent_Handler;
        GameSceneManager.ApplyBoosterEvent += ApplyBoosterEvent_Handler;
        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
    }

    protected virtual void OnDisable()
    {
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler; 
        GameSceneManager.PlayerReviveEvent -= PlayerReviveEvent_Handler; 
        GameSceneManager.HeartsAmountChangedEvent -= HeartsAmountChangedEvent_Handler;
        GameSceneManager.ApplyBoosterEvent -= ApplyBoosterEvent_Handler;
        GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
    }
    
    void ResetLevelEvent_Handler()
    {
        Setup(GameSceneManager.Instance.CurrentHearts);
    }
    
    void PlayerReviveEvent_Handler()
    {
        Setup(GameSceneManager.Instance.CurrentHearts);
    }

    void HeartsAmountChangedEvent_Handler(int amount)
    {
        ProcessHearts(amount);
    }

    void ApplyBoosterEvent_Handler(Booster booster)
    {
        if (booster is HeartBooster)
            StartAdditionalHeartAnimation();
    }

    void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
    {
        foreach (HeartView heart in HeartViews)
            heart.Hide();
    }

    void LevelClosedEvent_Handler()
    {
        foreach (HeartView heart in HeartViews)
            heart.Hide();
    }

    
    #endregion
}