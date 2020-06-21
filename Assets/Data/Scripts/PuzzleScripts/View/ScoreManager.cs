using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Puzzle
{
    public class ScoreManager : ManagerView
    {
        private int _score = 0;
        private int _tempScore = 0;
        private TextMeshProUGUI _scoreText;

        private string Score => $"Score: {_tempScore}";
        
        private string Text {
            
            set
            {
                float prevFontSize = _scoreText.fontSize;
                _scoreText.text = value;
                _scoreText.ForceMeshUpdate();
                float newFontSize = _scoreText.fontSize;

                if (!Mathf.Approximately(prevFontSize, newFontSize))
                    InvokeChangeSharedFontSize(newFontSize);
            }
        }

        void Awake()
        {
            _scoreText = GetComponent<TextMeshProUGUI>();
            Text = Score;
        }

        void Start()
        {
            InvokeChangeSharedFontSize(_scoreText.fontSize);
        }

        void AddScore(int score)
        {
            _score += score;
            StartCoroutine(ScrollScore());
        }

        private IEnumerator ScrollScore()
        {
            float delay = 0.5f / (_score - _tempScore);
            while (_tempScore != _score)
            {
                _tempScore++;
                Text = Score;
                ShowShort(_scoreText);
                yield return new WaitForSeconds(delay);
            }
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
            levelColorScheme.SetTextColor(_scoreText, true);
        }
        
    }
}