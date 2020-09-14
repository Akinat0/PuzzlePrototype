using System;
using System.Collections;
using System.Linq;
using Abu.Tools;
using Boo.Lang;
using Puzzle;
using UnityEngine;

public class HeartsHealthManager : MonoBehaviour
{
    [SerializeField] private HeartView HeartPrefab;
    [SerializeField] float HorizontalPadding = 0;
    [SerializeField] float HorizontalMargins = 0.05f;

    private static readonly int AnimationReset = Animator.StringToHash("Reset");

    List<HeartView> Hearts;
    
    int DefaultPlayerHp = 5;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            StartAdditionalHeartAnimation();
    }

    protected int Hp { get; set; }

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
    
    protected virtual void LoseHeart(int _Hp)
    {
        if (Hp < 0)
            return;

        HeartView heart = Hearts[Hp];

        if (heart != null)
            heart.Hide();
        else
            Debug.LogError($"Heart {Hp} is null");

        Hp--;
    }

    protected virtual void ResetHealth()
    {
        if(Hearts == null)
            CreateHearts();
        
        foreach (HeartView heart in Hearts)
            heart.gameObject.SetActive(true);

        if (HeartsAnimator != null)
            HeartsAnimator.SetTrigger(AnimationReset);

        Hp = Hearts.Count - 1;
        foreach (var heart in Hearts)
            heart.Show();
    }

    void CreateHearts()
    {
        if(Hearts == null)
            Hearts = new List<HeartView>();
        
        for (int i = 0; i < DefaultPlayerHp; i++)
        {
            HeartView heart = Instantiate(HeartPrefab, transform);
            heart.name = $"{HeartPrefab.name}_{i}";
            Hearts.Add(heart);
        }

        LayoutHearts();
    }

    void LayoutHearts()
    {
        int length = Hearts.Count;

        Vector2 camSize = ScreenScaler.CameraSize;


        float partOfScreen = 1.0f / length - HorizontalPadding - HorizontalMargins * 2 / length; 
        Vector3 targetScale = Vector3.one * ScreenScaler.FitHorizontalPart(Hearts.First().Background, partOfScreen);

        for (int i = 0; i < length; i++)
        {
            Hearts[i].transform.localScale = targetScale;
                
            Vector2 heartPosition = new Vector2
                (-camSize.x / 2 + Hearts[i].Size.x / 2 + camSize.x / length * i,
                -camSize.y / 2 + camSize.y * 0.05f);

            Hearts[i].transform.position = heartPosition;
        }

        float xOffset = (Mathf.Abs(Hearts.Last().transform.position.x) - Mathf.Abs(Hearts.First().transform.position.x)) / 2;

        foreach (Transform heartTransform in Hearts.Select(heart => heart.transform))
            heartTransform.SetX(heartTransform.position.x - xOffset);
    }

    void StartAdditionalHeartAnimation()
    {
        StartCoroutine(AdditionalHeartRoutine());
    }

    IEnumerator AdditionalHeartRoutine(float duration = 0.8f, Action finished = null)
    {
        HeartView additionalHeart = Instantiate(HeartPrefab, transform);
        additionalHeart.name = $"{HeartPrefab.name}_{Hearts.Count}";
        Hearts.Add(additionalHeart);

        Vector2 camSize = ScreenScaler.CameraSize;

        additionalHeart.transform.position = new Vector3(0, -camSize.y / 4.0f, 0);
        additionalHeart.transform.localScale = Vector3.one * ScreenScaler.FitHorizontalPart(additionalHeart.Background, 0.4f);

        additionalHeart.Show();

        int length = Hearts.Count;

        List<Vector3> sourceScales = new List<Vector3>();

        float partOfScreen = 1.0f / length - HorizontalPadding - HorizontalMargins * 2 / length;
        Vector3 targetScale = Vector3.one * ScreenScaler.FitHorizontalPart(Hearts.First().Background,
                                  partOfScreen);

        List<Vector2> sourcePositions = new List<Vector2>();
        List<Vector2> targetPositions = new List<Vector2>();
        
        for (int i = 0; i < Hearts.Count; i++)
        {
            Vector2 heartPosition = new Vector2
            (-camSize.x / 2 + Hearts[i].Size.x / Hearts[i].transform.localScale.x * targetScale.x / 2 + camSize.x / length * i,
                -camSize.y / 2 + camSize.y * 0.05f);

            targetPositions.Add(heartPosition);
            sourcePositions.Add(Hearts[i].transform.position);
            sourceScales.Add(Hearts[i].transform.localScale);
        }
        
        float xOffset = (Mathf.Abs(targetPositions.Last().x) - Mathf.Abs(targetPositions.First().x)) / 2;
        
        for (int i = 0; i < Hearts.Count; i++)
            targetPositions[i] -= Vector2.right * xOffset;
        
        yield return new WaitForSeconds(1.2f);
        
        float time = 0;

        while (time <= duration)
        {
            for (int i = 0; i < Hearts.Count; i++)
            {
                Hearts[i].transform.position = Vector3.Lerp(sourcePositions[i], targetPositions[i], time / duration);
                Hearts[i].transform.localScale = Vector3.Lerp(sourceScales[i], targetScale, time / duration);
            }

            time += Time.deltaTime;
            yield return null;
        }
        
        for (int i = 0; i < Hearts.Count; i++)
        {
            Hearts[i].transform.position = targetPositions[i];
            Hearts[i].transform.localScale = targetScale;
        }
        
        finished?.Invoke();
    }

    [ContextMenu("ShowBooster")]
    void ShowBooster()
    {
        StartAdditionalHeartAnimation();
    }
    
    #region event handlers
    
    protected virtual void OnEnable()
    {
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        GameSceneManager.PlayerReviveEvent += PlayerReviveEventHandler;
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        GameSceneManager.GameStartedEvent += GameStartedEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
    }

    protected virtual void OnDisable()
    {
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        GameSceneManager.PlayerReviveEvent -= PlayerReviveEventHandler;
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
        GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
        GameSceneManager.GameStartedEvent -= GameStartedEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
    }
    
    

    protected virtual void ResetLevelEvent_Handler()
    {
        ResetHealth();
    }

    protected virtual void PlayerReviveEventHandler()
    {
        ResetHealth();
    }

    protected virtual void PlayerLosedHpEvent_Handler(int _Hp)
    {
        LoseHeart(_Hp);
    }

    protected virtual void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
    {
        foreach (HeartView heart in Hearts)
            heart.Hide();
    }

    protected virtual void LevelClosedEvent_Handler()
    {
        foreach (HeartView heart in Hearts)
            heart.Hide();
    }

    protected virtual void GameStartedEvent_Handler()
    {
    }
    
    #endregion
}