using System;
using UnityEngine;

public partial class LevelConfig
{
    string Key => Name;
    
    #region difficulty level

    string DifficultyLevelKey => $"difficulty_{Key}";
    
    DifficultyLevel difficultyLevel;
    bool isDifficultyLevelLoaded = false;

    public DifficultyLevel DifficultyLevel
    {
        get
        {
            if (!isDifficultyLevelLoaded)
            {
                DifficultyLevel defaultDifficulty = SupportsDifficultyLevel(DifficultyLevel.Easy)
                    ? DifficultyLevel.Easy
                    : DifficultyLevel.Invalid;

                difficultyLevel = (DifficultyLevel) PlayerPrefs.GetInt(DifficultyLevelKey, (int) defaultDifficulty);
            }

            return difficultyLevel;
        }
        set
        {
            if(difficultyLevel == value)
                return;
            
            difficultyLevel = value;
            
            PlayerPrefs.SetInt(DifficultyLevelKey, (int) difficultyLevel);
            PlayerPrefs.Save();
        }
    }

    #endregion

    #region stars
    
    string StarsKey => $"stars_{Key}";

    int starsAmount;
    bool isStarsAmountLoaded;

    public event Action<int> StarsAmountChanged;
    
    public bool CanPlayLevel => Account.Stars.Has(Cost);
    
    public int StarsAmount
    {
        get
        {
            if (!isStarsAmountLoaded)
            {
                starsAmount = PlayerPrefs.GetInt(StarsKey, 0);
                isStarsAmountLoaded = true;
            }

            return starsAmount;
        }
        set
        {
            if (!isStarsAmountLoaded)
            {
                starsAmount = PlayerPrefs.GetInt(StarsKey, 0);
                isStarsAmountLoaded = true;
            }
            
            value = Mathf.Clamp(value, 0, 3);
            
            if (starsAmount == value)
                return;

            Account.Stars.Add(value - starsAmount);

            starsAmount = value;

            PlayerPrefs.SetInt(StarsKey, starsAmount);
            PlayerPrefs.Save();
            
            StarsAmountChanged?.Invoke(starsAmount);
        }
    }

    public bool ObtainFirstStar()
    {
        if (StarsAmount >= 1)
            return false;

        StarsAmount = 1;
        return true;
    }
    
    public bool ObtainSecondStar()
    {
        if (StarsAmount >= 2)
            return false;

        StarsAmount = 2;
        return true;
    }
    
    public bool ObtainThirdStar()
    {
        if (StarsAmount >= 3)
            return false;

        StarsAmount = 3;
        return true;
    }
    
    public bool TryObtainThirdStar(int hearts)
    {
        if (hearts < ThirdStarThreshold)
            return false;
        
        ObtainThirdStar();
        return true;
    }

    public bool HasStar(int starIndex) => StarsAmount >= starIndex; 

    #endregion
    
    #region score
    
    string ScoreKey => "Score " + Key;

    int score;
    bool isScoreLoaded = false;
    
    public int Score
    {
        get
        {
            if (!isScoreLoaded)
            {
                score = PlayerPrefs.GetInt(ScoreKey, 0);
                isScoreLoaded = true;
            }

            return score;
        }
        set
        {
            if (score == value)
                return;

            score = value;
            
            PlayerPrefs.SetInt(ScoreKey, Score);
            PlayerPrefs.Save();
        }
    }
    
    #endregion
}

