
using System;
using System.Collections.Generic;
using System.Linq;
using Abu.Tools;
using UnityEngine;

public class StarsView : MonoBehaviour
{
    [SerializeField] private Animator[] starAnimators;
    [SerializeField] private AudioClip[] starClips;
    
    private Dictionary<Animator, AnimationEventBehaviour> showEventBehaviours = new Dictionary<Animator, AnimationEventBehaviour>();
    private Dictionary<Animator, AnimationEventBehaviour> hideEventBehaviours = new Dictionary<Animator, AnimationEventBehaviour>();

    private static readonly int ShowID = Animator.StringToHash("Show");
    private static readonly int HighlightID = Animator.StringToHash("Highlight");
    private static readonly int HideID = Animator.StringToHash("Hide");
    private static readonly int InstantID = Animator.StringToHash("Instant");
    private static readonly int ActiveID = Animator.StringToHash("Active");

    static string ShowState => "Show";
    static string HideState => "Hide";

    bool playSound; 

    #region factory

    static StarsView prefab;
    static StarsView Prefab
    {
        get
        {
            if (prefab == null)
                prefab = Resources.Load<StarsView>("Prefabs/StarsView");
            return prefab;
        }
    }
    public static StarsView Create(Transform parent)
    {
        StarsView starsView = Instantiate(Prefab, parent);
        return starsView;
    }
        
    #endregion
    
    private void Awake()
    {
        //Calculate scale for single star as if it's a single element and then use it for whole view
        float singleImageScale =
            ScreenScaler.FitHorizontalPart(starAnimators.First().GetComponent<SpriteRenderer>(), 0.3f);

        transform.localScale = singleImageScale * Vector3.one;
        
        transform.localPosition = new Vector3(0, ScreenScaler.PartOfScreen(0.25f).y, 0);
        //

        if (!(starAnimators == null || starAnimators.Length == 0))
        {
            foreach (Animator starAnimator in starAnimators)
            {
                showEventBehaviours[starAnimator] = starAnimator.GetBehaviours<AnimationEventBehaviour>()
                    .First(_B => _B.StateId == ShowState);
                hideEventBehaviours[starAnimator] = starAnimator.GetBehaviours<AnimationEventBehaviour>()
                    .First(_B => _B.StateId == HideState);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            int indexClosure = i;

            showEventBehaviours[starAnimators[i]].OnStateExitEvent += _ =>
            {
                if (playSound)
                    SoundManager.Instance.PlayOneShot(starClips[indexClosure], 0.7f);
            };
        }

    }

    public void ShowStarsTogether(int stars)
    {
        playSound = false;
        
        stars = Mathf.Clamp(stars, 0, 3);
        
        for (int i = 0; i < 3; i++)
        {
            starAnimators[i].SetBool(InstantID, false);
            starAnimators[i].SetTrigger(ShowID);
            starAnimators[i].SetBool(ActiveID, i < stars);
        }
    }
    
    public void ShowStarsInstant(int stars)
    {
        playSound = false;
        
        stars = Mathf.Clamp(stars, 0, 3);

        for (int i = 0; i < 3; i++)
        {
            starAnimators[i].SetBool(InstantID, true);
            starAnimators[i].SetTrigger(ShowID);
            starAnimators[i].SetBool(ActiveID, i < stars);
        }
    }
    
    public void ShowStarsAnimation(int stars, Action finish = null)
    {
        playSound = true;
        
        stars = Mathf.Clamp(stars, 0, 3);

        //setup sounds and invokes
        for (int i = 0; i < 3; i++)
        {
            int indexClosure = i;
            
            //TODO check what does it implicitly captured closure really mean
            this.Invoke( () =>
                {
                    starAnimators[indexClosure].SetBool(InstantID, false);
                    starAnimators[indexClosure].SetTrigger(ShowID);
                    starAnimators[indexClosure].SetBool(ActiveID, indexClosure < stars);
                },
                0.7f * indexClosure);
        }

        bool hasInvoked = false;

        showEventBehaviours[starAnimators[stars - 1]].OnStateExitEvent += _ =>
        {
            if (hasInvoked)
                return;

            for (int i = 0; i < stars; i++)
                starAnimators[i].SetTrigger(HighlightID);

            hasInvoked = true;
            finish?.Invoke();
        };
    }

    public void HideStars(Action finish = null)
    {
        foreach (Animator starAnimator in starAnimators)
            starAnimator.SetTrigger(HideID);

        bool hasInvoked = false;
        
        hideEventBehaviours[starAnimators.Last()].OnStateExitEvent += _ =>
        {
            if (hasInvoked)
                return;
            
            hasInvoked = true;
            finish?.Invoke();
        };
    }
    
#if UNITY_EDITOR
    
    [ContextMenu("Hide stars")]
    public void EditorHideStars()
    {
        if (Application.IsPlaying(this))
            HideStars( () => Debug.LogError("Hide finished"));
    }
    
    [ContextMenu("3 stars")]
    public void EditorCallThreeStars()
    {
        if (Application.IsPlaying(this))
            ShowStarsAnimation(3, () => Debug.LogError("Animation finished"));
    }
    
    [ContextMenu("2 stars")]
    public void EditorCallTwoStars()
    {
        if (Application.IsPlaying(this))
            ShowStarsAnimation(2, () => Debug.LogError("Animation finished"));
    }
    
    [ContextMenu("1 star")]
    public void EditorCallOneStar()
    {
        if (Application.IsPlaying(this))
            ShowStarsAnimation(1, () => Debug.LogError("Animation finished"));
    }
    
    [ContextMenu("3 star instant")]
    public void EditorCallThreeStarsInstant()
    {
        if (Application.IsPlaying(this))
            ShowStarsInstant(3);
    }
    
    [ContextMenu("2 star instant")]
    public void EditorCallTwoStarsInstant()
    {
        if (Application.IsPlaying(this))
            ShowStarsInstant(2);
    }
    
    [ContextMenu("1 star instant")]
    public void EditorCallStarInstant()
    {
        if (Application.IsPlaying(this))
            ShowStarsInstant(1);
    }
#endif
}