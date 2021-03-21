using Abu.Tools;
using Abu.Tools.UI;
using DG.Tweening;
using Puzzle;
using TMPro;
using UnityEngine;

public class CoinSceneManager : ManagerView
{
    [SerializeField] WalletComponent Wallet;

    void Start()
    {
        AlphaSetter = alpha => Wallet.Alpha = alpha;
        AlphaGetter = () => Wallet.Alpha;
        
        TextGroup.Add(new TextObject(Wallet.Text.TextMesh));
        TextGroup.OnTextSizeResolved += Wallet.ForceUpdateLayout;
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
    {
        levelColorScheme.SetTextColor(Wallet.Text, true);
    }

    private void BalanceChangedEvent_Handler(int balance)
    {
        ShowShort();
        TextGroup.UpdateTextSize();
    }
    
    void PauseLevelEvent_Handler(bool pause)
    {
        if (pause)
            ShowInstant();
        else
            HideLong();
    }
}
