﻿using System.Collections;
using Abu.Tools.UI;
using PuzzleScripts;
using UnityEngine;

namespace Puzzle
{
    public class ScoreManager : GameText
    {
        int score;
        int tempScore;
        TextComponent scoreText;

        string Score => $"Score: {tempScore}";
        
        string Text {
            
            set => scoreText.Text = value;
        }

        void Awake()
        {
            scoreText = GetComponent<TextComponent>();
            AlphaSetter = alpha => scoreText.Alpha = alpha;
            AlphaGetter = () => scoreText.Alpha;
            Text = Score;
            
            TextGroup.Add(new TextObject(scoreText.TextMesh));
        }

        void AddScore(int score)
        {
            this.score += score;
            StartCoroutine(ScrollScore());
            
        }

        IEnumerator ScrollScore()
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

        void OnEnable()
        {
            GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
            GameSceneManager.PlayerDiedEvent += PlayerDiedEvent_Handler;
            GameSceneManager.EnemyDiedEvent += EnemyDiedEvent_Handler;
            GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        }

        void OnDisable()
        {
            GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
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
        
        void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
        {
            levelColorScheme.SetTextColor(scoreText, true);
        }
        
    }
}