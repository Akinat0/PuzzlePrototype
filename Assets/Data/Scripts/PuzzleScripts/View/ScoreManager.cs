﻿using System.Collections;
using DG.Tweening;
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
            
            set
            {
                float prevFontSize = scoreText.fontSize;
                scoreText.text = value;
                scoreText.ForceMeshUpdate();
                float newFontSize = scoreText.fontSize;

                if (!Mathf.Approximately(prevFontSize, newFontSize))
                    InvokeChangeSharedFontSize(newFontSize);
            }
        }

        void Awake()
        {
            scoreText = GetComponent<TextMeshProUGUI>();
            Text = Score;
        }

        void Start()
        {
            InvokeChangeSharedFontSize(scoreText.fontSize);
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
                ShowShort(scoreText); //TODO I don't like the solution
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
            HideInstant(scoreText);
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
                ShowInstant(scoreText);
            else
                HideLong(scoreText);
        }
        
        protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
        {
            levelColorScheme.SetTextColor(scoreText, true);
        }
        
    }
}