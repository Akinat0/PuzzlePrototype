using Abu.Tools.UI;
using UnityEngine;
using Puzzle;

public class AttemptCounter : GameText
{
    int attemptNumber;

    [SerializeField] TextComponent TextPlaceholder;

    void Awake()
    {
        attemptNumber = 0;
        AlphaSetter = alpha => TextPlaceholder.Alpha = alpha;
        AlphaGetter = () => TextPlaceholder.Alpha;
    }

    void OnEnable()
    {
        GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }

    void OnDisable()
    {
        GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
    }

    void ResetLevelEvent_Handler()
    {
        attemptNumber++;
        TextPlaceholder.Text = $"Attempt #{attemptNumber}";
        ShowInstant();
        HideLong();
    }

    void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        TextPlaceholder.Color = levelColorScheme.TextColor2;
    }
}
