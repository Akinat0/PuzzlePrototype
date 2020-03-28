using DG.Tweening;
using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class CoinSceneManager : ManagerView
{

    [SerializeField] private Text coinText;

    private void Start()
    {
        Color textColor = coinText.color;
        textColor.a = 0;
        coinText.color = textColor;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Account.BalanceChangedEvent += BalanceChangedEvent_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Account.BalanceChangedEvent -= BalanceChangedEvent_Handler;
    }

    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        coinText.color = levelColorScheme.TextColor2;
    }

    private void BalanceChangedEvent_Handler(int balance)
    {
        coinText.text = balance.ToString();
        
        coinText.DOKill();
        var fadeOutTween = coinText.DOFade(1.0f, 1.0f);
        fadeOutTween.onComplete += () => coinText.DOFade(0.0f, 1.0f);
    }
    
}
