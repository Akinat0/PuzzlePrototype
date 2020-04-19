using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

namespace Puzzle
{
    public class ScoreManager : ManagerView
    {
        private int _score = 0;
        private Text _scoreText;

        public string Score => $"Score: {_score}";

        void Awake()
        {
            _scoreText = GetComponent<Text>();
            _scoreText.text = Score;
        }

        void AddScore(int score)
        {
            _score += score;
            _scoreText.text = Score;
            ShowShort(_scoreText);
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
            GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
            GameSceneManager.PlayerDiedEvent -= PlayerDiedEvent_Handler;
            GameSceneManager.EnemyDiedEvent -= EnemyDiedEvent_Handler;
            GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        }
        
        void ResetLevelEvent_Handler()
        {
            _score = 0;
        }

        void PlayerDiedEvent_Handler()
        {
            SaveScore();
        }

        void EnemyDiedEvent_Handler(int score)
        {
            AddScore(score);
        }

        void PauseLevelEvent_Handler(bool pause)
        {
            if (pause)
                ShowInstant(_scoreText);
            else
                HideLong(_scoreText);
        }
        
        protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
        {
            _scoreText.color = levelColorScheme.TextColor2;
        }
    }
}