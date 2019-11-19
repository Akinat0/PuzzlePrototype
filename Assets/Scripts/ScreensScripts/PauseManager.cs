using System;
using System.Collections;
using System.Collections.Generic;
using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    private bool _paused = false;
    public bool Paused => _paused;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void OnPause()
    {
        if(!Paused)
            PauseTheGame();
        else
        {
            ResumeTheGame();
        }
    }

    private void PauseTheGame()
    {
        _paused = true;
        Time.timeScale = 0;
        GameSceneManager.Instance.soundManager.PauseSounds(true);
        pauseMenu.SetActive(true);
    }

    private void ResumeTheGame()
    {
        Time.timeScale = 1;
        _paused = false;
        GameSceneManager.Instance.soundManager.PauseSounds(false);
        pauseMenu.SetActive(false);
    }

    public void VolumeSliderValueChanged(float value)
    {
        value = Mathf.Clamp01(value);
        GameSceneManager.Instance.soundManager.SetVolume(value);
    }
}
