using DG.Tweening;
using Puzzle;
using TMPro;
using UnityEngine;

public class CoinSceneManager : ManagerView
{
    [SerializeField] private TextMeshProUGUI CoinText;

    private void Start()
    {
        CoinText.text = Account.Coins.ToString();
        Color textColor = CoinText.color;
        CoinText.color = textColor;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Account.BalanceChangedEvent += BalanceChangedEvent_Handler;
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        ChangeSharedFontSize += ChangeSharedFontSize_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Account.BalanceChangedEvent -= BalanceChangedEvent_Handler;
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        ChangeSharedFontSize -= ChangeSharedFontSize_Handler;
    }

    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        CoinText.color = levelColorScheme.TextColor2;
    }

    private void BalanceChangedEvent_Handler(int balance)
    {
        CoinText.text = balance.ToString();
        ShowShort(CoinText);
    }
    
    void PauseLevelEvent_Handler(bool pause)
    {
        if (pause)
            ShowInstant(CoinText);
        else
            HideLong(CoinText);
    }
    
    private void ChangeSharedFontSize_Handler(float sharedFontSize)
    {
        if (!Mathf.Approximately(CoinText.fontSize, sharedFontSize))
            CoinText.fontSize = sharedFontSize;
    }
}
