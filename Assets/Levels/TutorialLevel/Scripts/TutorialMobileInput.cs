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
        TutorialManager.OnTutorialInputEnabled += OnTutorialInputEnabled_Handler;
        TutorialManager.OnTutorialInputDisabled += OnTutorialInputDisabled_Handler;
        TutorialManager.OnTutorialRestart += OnTutorialRestart_Handler;
    }
    
    protected override void OnDisable()
    {
        TutorialManager.OnTutorialInputEnabled -= OnTutorialInputEnabled_Handler;
        TutorialManager.OnTutorialInputDisabled -= OnTutorialInputDisabled_Handler;
        TutorialManager.OnTutorialRestart -= OnTutorialRestart_Handler;
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
