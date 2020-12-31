using System.Collections;
using Abu.Tools;
using Abu.Tools.UI;
using DG.Tweening;
using PuzzleScripts;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Puzzle
{
    public class ScoreManager : ManagerView
    {
        private int score = 0;
        private int tempScore = 0;
        private TextMeshProUGUI scoreText;

        private string Score => $"Score: {tempScore}";
        
        private string Text {
            
            set => scoreText.text = value;
        }

        void Awake()
        {
            scoreText = GetComponent<TextMeshProUGUI>();
            AlphaSetter = alpha => scoreText.alpha = alpha;
            AlphaGetter = () => scoreText.alpha;
            Text = Score;
            
            TextGroup.Add(new TextObject(scoreText));
        }

        void AddScore(int score)
        {
            this.score += score;
            StartCoroutine(ScrollScore());
            
        }

        private IEnumerator ScrollScore()
        {
            float delay = 0.5f / (score - tempScore);
            while (tempScore < score)
            {
                tempScore++;
                Text = Score;
                TextGroup.UpdateTextSize();
                
                ShowShort(); //TODO I don't like the solution
                yield return new WaitForSeconds(delay);
            }
        }
        
        void SaveScore()
        {
            PlayerPrefs.SetInt("score", score);
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
            score = 0;
            tempScore = 0;
            StopAllCoroutines();
            HideInstant();
        }

        void PlayerDiedEvent_Handler()
        {
            SaveScore();
        }

        void EnemyDiedEvent_Handler(EnemyBase enemyBase)
        {
            AddScore(enemyBase.Score);
        }

        void PauseLevelEvent_Handler(bool pause)
        {
            if (pause)
                ShowInstant();
            else
                HideLong();
        }
        
        protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
        {
            levelColorScheme.SetTextColor(scoreText, true);
        }
        
    }
}