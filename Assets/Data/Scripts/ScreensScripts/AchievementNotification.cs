using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AchievementNotification : TextButtonComponent
{
    static readonly int ShowID = Animator.StringToHash("Show");
    
    Animator animator;
    AnimationEventBehaviour showAnimation;

    Action completedAction;

    void Awake()
    {
        animator = GetComponent<Animator>();
        showAnimation = AnimationEventBehaviour.FindState(animator, "show");
        showAnimation.OnComplete += InvokeShowCompleted;
    }
    
    public void Setup(Achievement achievement)
    {
        Icon.Image.sprite = achievement.Icon;
        Text = achievement.Name;
    }

    public void Show(Action finished)
    {
        InvokeShowCompleted();
        completedAction += finished;
        animator.SetTrigger(ShowID);
    }

    void InvokeShowCompleted()
    {
        Action action = completedAction;
        completedAction = null;
        action?.Invoke();
    }
}
