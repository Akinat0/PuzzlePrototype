
using UnityEngine;

public class RichBitchAchievement : Achievement
{
    
    public RichBitchAchievement()
    {
        Progress = Account.Coins;
        Account.BalanceChangedEvent += BalanceChangedEvent_Handler;
    }
    
    public override string Name => "Rich Bitch";
    public override float TargetProgress => 1000;
    
    void BalanceChangedEvent_Handler(int balance)
    {
        Progress = balance;
    }
    
}
