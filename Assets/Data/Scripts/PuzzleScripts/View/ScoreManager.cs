using UnityEngine.UI;
using UnityEngine;

namespace Puzzle
{
    public class ScoreManager : ManagerView
    {

        private int _score = 0;
        private Text _scoreText;

        void Start()
        {
            _scoreText = GetComponent<Text>();
            AddScore(0);
        }

        void AddScore(int score)
        {
            _score += score;
            _scoreText.text = "Score: " + _score;
        }

        void SaveScore()
        {
            PlayerPrefs.SetInt("score", _score);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
            GameSceneManager.PlayerDiedEvent += PlayerDiedEvent_Handler;
            GameSceneManager.EnemyDiedEvent += EnemyDiedEvent_Handler;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
            GameSceneManager.PlayerDiedEvent -= PlayerDiedEvent_Handler;
            GameSceneManager.EnemyDiedEvent -= EnemyDiedEvent_Handler;
        }
        
        void ResetLevelEvent_Handler()
        {
            _score = 0;
            AddScore(_score);
        }

        void PlayerDiedEvent_Handler()
        {
            SaveScore();
        }

        void EnemyDiedEvent_Handler(int score)
        {
            AddScore(score);
        }
        
        protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
        {
            _scoreText.color = levelColorScheme.TextColor2;
        }
    }
}