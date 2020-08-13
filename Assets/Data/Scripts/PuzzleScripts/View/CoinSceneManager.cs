using Abu.Tools.UI;
using DG.Tweening;
using Puzzle;
using TMPro;
using UnityEngine;

public class CoinSceneManager : ManagerView
{
    [SerializeField] private WalletComponent Wallet;
    
    void Start()
    {
        //We must be sure that TMP has already updated
        TextGroup.Add(Wallet.Text, true);
        
        AlphaSetter = alpha => Wallet.Alpha = alpha;
        AlphaGetter = () => Wallet.Alpha;
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        Account.BalanceChangedEvent += BalanceChangedEvent_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        Account.BalanceChangedEvent -= BalanceChangedEvent_Handler;
    }

    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    { }

    private void BalanceChangedEvent_Handler(int balance)
    {
        ShowShort();
        
        TextGroup?.ResolveTextSize();
    }
    
    void PauseLevelEvent_Handler(bool pause)
    {
        if (pause)
            ShowInstant();
        else
            HideLong();
    }
    
    private void ChangeSharedFontSize_Handler(float sharedFontSize)
    {
        if (!Mathf.Approximately(Wallet.Text.fontSize, sharedFontSize))
            Wallet.Text.fontSize = sharedFontSize;
    }
}
