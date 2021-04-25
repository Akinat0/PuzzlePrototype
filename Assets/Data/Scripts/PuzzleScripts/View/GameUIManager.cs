using ScreensScripts;
using UnityEngine;

namespace Puzzle
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] CompleteScreen completeScreen;
        [SerializeField] ReplayScreen   replayScreen;

        void OnEnable()
        {
            GameSceneManager.PlayerDiedEvent     += PlayerDiedEvent_Handler;
            GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        }

        void OnDisable()
        {
            GameSceneManager.PlayerDiedEvent     -= PlayerDiedEvent_Handler;
            GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
        }

        void PlayerDiedEvent_Handler()
        {
            replayScreen.Show();
        }

        void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
        {
            completeScreen.Show();
        }
    }
}