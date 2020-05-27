
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

    private static readonly int Show = Animator.StringToHash("Show");
    private static readonly int Highlight = Animator.StringToHash("Highlight");
    private static readonly int Hide = Animator.StringToHash("Hide");

    public static string ShowState => "Show";
    public static string HideState => "Hide";

    private int ShownStars { get; set; }

    private void Start()
    {
        //Calculate scale for single star as if it's a single element and then use it for hole view
        float singleImageScale =
            ScreenScaler.FitHorizontalPart(starAnimators.First().GetComponent<SpriteRenderer>(), 0.35f);

        transform.localScale = singleImageScale * Vector3.one;
        
        
        transform.localPosition = new Vector3(0, ScreenScaler.PartOfScreen(0.15f).y, 0);
        //

        if (!(starAnimators == null || starAnimators.Length == 0))
            foreach (Animator starAnimator in starAnimators)
            {
                showEventBehaviours[starAnimator] = starAnimator.GetBehaviours<AnimationEventBehaviour>()
                    .First(_B => _B.StateId == ShowState);
                hideEventBehaviours[starAnimator] = starAnimator.GetBehaviours<AnimationEventBehaviour>()
                    .First(_B => _B.StateId == HideState);
            }

    }
    
    public void ShowStars(int stars, Action finish)
    {
        ShownStars = stars;
        
        if (stars > 0)
        {
            //setup sounds
            for (int i = 0; i < stars; i++)
            {
                int indexClosure = i;
                showEventBehaviours[starAnimators[i]].OnStateCompleteEvent += _ =>
                    SoundManager.Instance.PlayOneShot(starClips[indexClosure], 0.7f);
            }
            
            for (int i = 0; i < stars - 1; i++)
            {
                int indexClosure = i;
                
                //Set next animator show trigger after completing
                showEventBehaviours[starAnimators[i]].OnStateCompleteEvent += _ => starAnimators[indexClosure + 1].SetTrigger(Show);
            }
            
            //Set trigger to the last animator to play highlight
            showEventBehaviours[starAnimators[stars - 1]].OnStateCompleteEvent += _ =>
            {
                for (int i = 0; i < stars; i++)
                    starAnimators[i].SetTrigger(Highlight);
                
                finish?.Invoke();
            };

            //Start first star animation
            starAnimators.First().SetTrigger(Show);
        }

    }

    public void HideStars(Action finish)
    {
        if (ShownStars == 0)
        {
            finish?.Invoke();
            return;
        }

        for (int i = 0; i < ShownStars; i++)
            starAnimators[i].SetTrigger(Hide);
        
        hideEventBehaviours[starAnimators.First()].OnStateExitEvent += _ => finish?.Invoke();
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
            ShowStars(3, () => Debug.LogError("Animation finished"));
    }
    
    [ContextMenu("2 stars")]
    public void EditorCallTwoStars()
    {
        if (Application.IsPlaying(this))
            ShowStars(2, () => Debug.LogError("Animation finished"));
    }
    
    [ContextMenu("1 star")]
    public void EditorCallOneStar()
    {
        if (Application.IsPlaying(this))
            ShowStars(1, () => Debug.LogError("Animation finished"));
    }
#endif
}