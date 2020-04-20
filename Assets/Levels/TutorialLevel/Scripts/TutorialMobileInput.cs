using Puzzle;
using UnityEngine;

public class TutorialMobileInput : MobileGameInput
{
    private void Start()
    {
        //Disable input on start
        Condition = false;
    }

    protected override void OnEnable()
    {
        TutorialTimelineManager.OnTutorialInputEnabled += OnTutorialInputEnabled_Handler;
        TutorialTimelineManager.OnTutorialInputDisabled += OnTutorialInputDisabled_Handler;
        TutorialTimelineManager.OnTutorialRestart += OnTutorialRestart_Handler;
    }
    
    protected override void OnDisable()
    {
        TutorialTimelineManager.OnTutorialInputEnabled -= OnTutorialInputEnabled_Handler;
        TutorialTimelineManager.OnTutorialInputDisabled -= OnTutorialInputDisabled_Handler;
        TutorialTimelineManager.OnTutorialRestart -= OnTutorialRestart_Handler;
    }

    void OnTutorialInputEnabled_Handler()
    {
        Condition = true;
    }
    
    void OnTutorialInputDisabled_Handler()
    {
        Condition = false;
    }
    
    void OnTutorialRestart_Handler()
    {
        Condition = false;
    }
}
