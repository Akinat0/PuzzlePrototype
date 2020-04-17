﻿using System;
using Puzzle;
using UnityEngine;

public class HeartsHealthManager : HealthManagerBase
{

    [SerializeField] private Animator heartsAnimator;
    [SerializeField] private HeartView[] hearts;
    
    private static readonly int AnimationReset = Animator.StringToHash("Reset");

    private void Start()
    { 
        Hp = hearts.Length - 1;
        
        foreach (HeartView heart in hearts)
            heart.gameObject.SetActive(false);
    }

    protected override void LoseHeart(int _Hp)
    {
        if (Hp < 0)
            return;

        HeartView heart = hearts[Hp];
        
        if(heart!=null)
            heart.Hide();
        else
            Debug.LogError($"Heart {Hp} is null");
        
        Hp--;
    }

    protected override void ResetHealth()
    {
        foreach (HeartView heart in hearts)
            heart.gameObject.SetActive(true);
        
        if(heartsAnimator != null)
            heartsAnimator.SetTrigger(AnimationReset);
        
        Hp = hearts.Length - 1;
        foreach (var heart in hearts)
            heart.Show();
    }

    protected override void LevelCompletedEvent_Handler()
    {
        base.LevelCompletedEvent_Handler();
        foreach (HeartView heart in hearts)
            heart.Hide();
    }

}