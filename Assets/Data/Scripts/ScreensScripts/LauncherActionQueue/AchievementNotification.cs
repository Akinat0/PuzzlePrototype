using System;
using Puzzle;
using ScreensScripts;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AchievementNotification : TextButtonComponent
{
    static readonly int ShowID = Animator.StringToHash("Show");

    Animator animator;
    AnimationEventBehaviour completeAnimation;

    Action completedAction;

    Achievement achievement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        completeAnimation = AnimationEventBehaviour.FindState(animator, "complete");
        completeAnimation.OnComplete += InvokeAnimationCompleted;

        OnClick += ProcessClick;
    }

    public void Setup(Achievement achievement)
    {
        Icon.Image.sprite = achievement.Icon;
        this.achievement = achievement;
        Text = achievement.Name;
    }

    public void Show(Action finished)
    {
        Interactable = true;
        
        InvokeAnimationCompleted();
        
        completedAction += finished;
        animator.SetBool(ShowID, true);
    }

    void Hide()
    {
        Interactable = false;
        animator.SetBool(ShowID, false);
    }

    void InvokeAnimationCompleted()
    {
        Action action = completedAction;
        completedAction = null;
        animator.SetBool(ShowID, false);
        action?.Invoke();
    }

    void ProcessClick()
    {
        Hide();
        LauncherUI.Instance.InvokeShowAchievementScreen(achievement);
    }
}
