using DG.Tweening;
using Puzzle;
using TMPro;
using UnityEngine;

public class CoinSceneManager : ManagerView
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        coinText.text = Account.Coins.ToString();
        Color textColor = coinText.color;
        coinText.color = textColor;
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
        coinText.color = levelColorScheme.TextColor2;
    }

    private void BalanceChangedEvent_Handler(int balance)
    {
        coinText.text = balance.ToString();
        ShowShort(coinText);
    }
    
    void PauseLevelEvent_Handler(bool pause)
    {
        if (pause)
            ShowInstant(coinText);
        else
            HideLong(coinText);
    }
    
    private void ChangeSharedFontSize_Handler(float sharedFontSize)
    {
        if (!Mathf.Approximately(coinText.fontSize, sharedFontSize))
            coinText.fontSize = sharedFontSize;
    }
}
