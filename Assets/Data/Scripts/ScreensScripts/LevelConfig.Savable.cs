using UnityEngine;

public partial class LevelConfig
{
    string Key => Name;
    
    #region difficulty level

    string DifficultyLevelKey => "Difficulty " + Key;
    
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
    
    string StarsKey => "Stars " + Key;

    int starsAmount;
    bool isStarsAmountLoaded = false;
    
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
            value = Mathf.Clamp(value, 0, 3);
            
            if (starsAmount == value)
                return;

            starsAmount = value;
            
            PlayerPrefs.SetInt(StarsKey, starsAmount);
            PlayerPrefs.Save();
        }
    }
    
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

