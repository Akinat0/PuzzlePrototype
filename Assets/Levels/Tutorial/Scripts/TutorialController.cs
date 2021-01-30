using Puzzle;
using Tutorial.GameActions;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] AudioClip TutorialTheme;
    TutorialSceneManager SceneManager => GameSceneManager.Instance as TutorialSceneManager;
    GameActionQueue queue;
    GameEnemiesCounter enemiesCounter;
    BubbleDialog dialog;
    
    void Start()
    {
        enemiesCounter = new GameEnemiesCounter();
        queue = gameObject.AddComponent<GameActionQueue>();
    }

    void OnEnable()
    {
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelCloseEvent_Handler;
    }

    void OnDisable()
    {
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelCloseEvent_Handler;
    }

    void StartFirstStage()
    {
        AddGameAction(new TutorialDelayGameAction(0.0001f));
        AddGameAction(new TutorialCustomGameAction(PlayAudio));
        AddGameAction(new TutorialDelayGameAction(1f));
        AddGameAction(new TutorialShowDialogGameAction(dialog, "Hi, Friend!", 2f));
        AddGameAction(new TutorialDelayGameAction(1.5f));
        AddGameAction(new TutorialShowDialogGameAction(dialog,"I will tell you how to play this game", 5f));
        AddGameAction(new TutorialDelayGameAction(0.75f));
        //Puzzles
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Left, 1.3f, true));
        AddGameAction(new TutorialDelayGameAction(0.75f));
        AddGameAction(new TutorialShowDialogGameAction(dialog,"Look..", 2f));
        AddGameAction(new TutorialDelayGameAction(2));
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Up, 1.3f, false));
        AddGameAction(new TutorialDelayGameAction(2));
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Right, 1.3f, true));
        AddGameAction(new TutorialDelayGameAction(1));
        AddGameAction(new TutorialWaitForAllEnemiesDiedAction(enemiesCounter));
        
        //Shits
        AddGameAction(new TutorialShitEnemyGameAction(Side.Left, 1.3f));
        AddGameAction(new TutorialDelayGameAction(0.1f));
        AddGameAction(new TutorialShowDialogGameAction(dialog, "Perfect! Let's check some other types of enemies ;)", 3f));
        AddGameAction(new TutorialDelayGameAction(1.5f));
        AddGameAction(new TutorialShitEnemyGameAction(Side.Down, 1.3f));
        AddGameAction(new TutorialDelayGameAction(1.5f));
        AddGameAction(new TutorialShitEnemyGameAction(Side.Right, 1.3f));
        AddGameAction(new TutorialWaitForAllEnemiesDiedAction(enemiesCounter));
        
        //Start second stage
        AddGameAction(new TutorialCustomGameAction(StartSecondStage));
    }

    void StartSecondStage()
    {
        SceneManager.InvokeTutorialNextStage();
        StartFirstFigure();
    }

    void StartFirstFigure()
    {
        void RestartFigure()
        {
            queue.Reset();
            StartFirstFigure();
        }
        
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Down, 1.8f, false));
        AddGameAction(new TutorialDelayGameAction(1.5f));
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Up, 1.8f, true));
        AddGameAction(new TutorialDelayGameAction(1.5f));
        AddGameAction(new TutorialShitEnemyGameAction(Side.Up, 1.8f));
        
        AddGameAction(new TutorialRestartFigureOrWaitAction(enemiesCounter, RestartFigure));
        AddGameAction(new TutorialDelayGameAction(0.001f));
        AddGameAction(new TutorialCustomGameAction(StartSecondFigure));
    }
    
    void StartSecondFigure()
    {
        void RestartFigure()
        {
            queue.Reset();
            StartSecondFigure();
        }
        
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Left, 1.8f, false));
        AddGameAction(new TutorialDelayGameAction(1f));
        AddGameAction(new TutorialShitEnemyGameAction(Side.Left, 1.8f));
        AddGameAction(new TutorialDelayGameAction(1f));
        AddGameAction(new TutorialShitEnemyGameAction(Side.Left, 1.8f));
        
        AddGameAction(new TutorialDelayGameAction(1.5f));
        
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Right, 1.8f, false));
        AddGameAction(new TutorialDelayGameAction(1f));
        AddGameAction(new TutorialShitEnemyGameAction(Side.Right, 1.8f));
        AddGameAction(new TutorialDelayGameAction(1f));
        AddGameAction(new TutorialShitEnemyGameAction(Side.Right, 1.8f));
        
        AddGameAction(new TutorialRestartFigureOrWaitAction(enemiesCounter, RestartFigure));
        AddGameAction(new TutorialDelayGameAction(0.001f));
        AddGameAction(new TutorialCustomGameAction(StartThirdFigure));
    }
    
    void StartThirdFigure()
    {
        void RestartFigure()
        {
            queue.Reset();
            StartThirdFigure();
        }
        
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Left, 1.9f, true));
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Down, 1.9f, true));
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Up, 1.9f, true));
        AddGameAction(new TutorialPuzzleEnemyGameAction(Side.Right, 1.9f, true));
        
        AddGameAction(new TutorialRestartFigureOrWaitAction(enemiesCounter, RestartFigure));
        
        AddGameAction(new TutorialDelayGameAction(2));
        
        AddGameAction(new TutorialCustomGameAction(() => SceneManager.InvokeLevelCompleted()));
    }

    void AddGameAction(GameAction action)
    {
        queue.Add(action);
    }
    
    void PlayAudio()
    {
        SceneManager.InvokePlayAudio(new LevelPlayAudioEventArgs(TutorialTheme, true,
            AnimationCurve.Constant(0, 1, 1)));
    }
    
    #region event handlers

    void ResetLevelEvent_Handler()
    {
        if(dialog == null) 
            dialog = BubbleDialog.Create(GameSceneManager.Instance.Player.PlayerView);
        
        queue.Reset();
        StartFirstStage();
    }

    void LevelCloseEvent_Handler()
    {
        dialog.Hide();
        queue.Reset();
    }
    
    #endregion
}
