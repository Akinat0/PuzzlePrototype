
using System.Collections.Generic;
using UnityEngine;

public abstract class Achievement
{
    public static Achievement[] CreateAllAchievements()
    {
        List<Achievement> achievements = new List<Achievement>();

        achievements.Add(new TutorialAchievement());
        achievements.Add(new RichBitchAchievement());
        
        return achievements.ToArray();
    }

    protected Achievement()
    {
        int IsReceivedCode = PlayerPrefs.GetInt(IsReceivedKey, 0);
        
        //Convert int to bool
        IsReceived = IsReceivedCode != 0; 

        Progress = PlayerPrefs.GetFloat(ProgressKey, 0.0f);
    }

    public abstract string Name { get; } 

    public abstract float TargetProgress { get; }

    string Key => Name;

    string ProgressKey => Key + "Progress";
    string IsReceivedKey => Key + "IsReceived";

    public bool IsReceived;

    float progress;

    public float Progress
    {
        get => progress;

        set
        {
            if (IsReceived)
            {
                progress = TargetProgress;
                return;
            }
            
            progress = Mathf.Clamp(value, 0, TargetProgress);
            ProcessProgress();
        }
    }

    protected virtual void ProcessProgress()
    {
        if (Progress >= TargetProgress)
            EarnAchievement();
        
        PlayerPrefs.SetFloat(ProgressKey, Progress);
        PlayerPrefs.Save();
    }

    protected virtual void EarnAchievement()
    {
        IsReceived = true;
        PlayerPrefs.SetInt(IsReceivedKey, 1);
        PlayerPrefs.Save();
    }
}


