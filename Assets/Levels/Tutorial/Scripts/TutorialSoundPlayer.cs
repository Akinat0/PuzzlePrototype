using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSoundPlayer : LevelSoundPlayer
{
    protected override void OnEnable()
    {
        base.OnEnable();
        TutoriaScenelManager.OnStopTutorial += OnStopTutorial_Handler;
        TutoriaScenelManager.OnTutorialRestart += OnTutorialReset_Handler;        TutoriaScenelManager.OnTutorialNextStage -= OnTutorialNextStage_Handler;
        TutoriaScenelManager.OnTutorialNextStage += OnTutorialNextStage_Handler;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        TutoriaScenelManager.OnStopTutorial -= OnStopTutorial_Handler;
        TutoriaScenelManager.OnTutorialRestart -= OnTutorialReset_Handler;
        TutoriaScenelManager.OnTutorialNextStage -= OnTutorialNextStage_Handler;
    }

    void OnStopTutorial_Handler(bool paused)
    {
        Pause(paused);
    }
    
    void OnTutorialReset_Handler()
    {
        ClearAudio();
    }
    
    void OnTutorialNextStage_Handler()
    {
        ClearAudio();
    }
}
