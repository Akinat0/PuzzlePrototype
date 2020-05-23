using Abu.Tools;
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
        TouchRegistered += TouchRegistered_Handler;
        TutoriaScenelManager.OnTutorialInputEnabled += OnTutorialInputEnabled_Handler;
        TutoriaScenelManager.OnTutorialInputDisabled += OnTutorialInputDisabled_Handler;
        GameSceneManager.ResetLevelEvent += OnRestartLevel_Handler;
    }
    
    protected override void OnDisable()
    {
        TouchRegistered -= TouchRegistered_Handler;
        TutoriaScenelManager.OnTutorialInputEnabled -= OnTutorialInputEnabled_Handler;
        TutoriaScenelManager.OnTutorialInputDisabled -= OnTutorialInputDisabled_Handler;
        GameSceneManager.ResetLevelEvent -= OnRestartLevel_Handler;
    }

    void OnTutorialInputEnabled_Handler()
    {
        Condition = true;
    }
    
    void OnTutorialInputDisabled_Handler()
    {
        Condition = false;
    }
    
    void TouchRegistered_Handler(Touch touch)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);
        VFXManager.Instance.CallTapRippleEffect(position);
    }
    
    void OnRestartLevel_Handler()
    {
        Condition = false;
    }
}
