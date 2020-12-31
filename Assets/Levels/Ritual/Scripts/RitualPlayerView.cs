using Puzzle;
using UnityEngine;

public class RitualPlayerView : SkinPlayerView
{
    static readonly int MagicID = Animator.StringToHash("Magic");
    static readonly int EvilID = Animator.StringToHash("Evil");

    protected override void OnEnable()
    {
        base.OnEnable();
        GameSceneManager.TimelineEvent += TimelineEvent_Handler;
        GameSceneManager.ResetLevelEvent += RestoreLevelEvent_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameSceneManager.TimelineEvent -= TimelineEvent_Handler;
        GameSceneManager.ResetLevelEvent -= RestoreLevelEvent_Handler;
    }

    protected override void RestoreView()
    {
        base.RestoreView();

        Animator.SetBool(MagicID, false);
        Animator.SetBool(EvilID, true);
    }

    private void RestoreLevelEvent_Handler()
    {
        Animator.SetBool(MagicID, false);
        Animator.SetBool(EvilID, true);
    }
    void TimelineEvent_Handler(string eventData)
    {
        switch (eventData)
        {
            case "ritual_magic_started_event":
                Animator.SetBool(MagicID, true);
                break;
            case "ritual_magic_ended_event":
                Animator.SetBool(MagicID, false);
                break;
            case "ritual_evil_event":
                Animator.SetBool(EvilID, true);
                break;
            case "ritual_happy_event":
                Animator.SetBool(EvilID, false);
                break;
        }
    }
}
