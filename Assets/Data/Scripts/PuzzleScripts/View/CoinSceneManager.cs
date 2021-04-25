using Abu.Tools.UI;
using Puzzle;
using UnityEngine;

public class CoinSceneManager : GameText
{
    [SerializeField] WalletComponent Wallet;

    void Start()
    {
        AlphaSetter = alpha => Wallet.Alpha = alpha;
        AlphaGetter = () => Wallet.Alpha;
        
        TextGroup.Add(new TextObject(Wallet.Text.TextMesh));
        TextGroup.OnTextSizeResolved += Wallet.ForceUpdateLayout;
    }
    
    void OnEnable()
    {
        GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        Account.BalanceChangedEvent += BalanceChangedEvent_Handler;
    }

    void OnDisable()
    {
        GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        Account.BalanceChangedEvent -= BalanceChangedEvent_Handler;
    }

    void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetTextColor(Wallet.Text, true);
    }

    void BalanceChangedEvent_Handler(int balance)
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
