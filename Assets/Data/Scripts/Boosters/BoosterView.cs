using System.Linq;
using Abu.Tools.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Data.Scripts.Boosters
{
    public class BoosterView : ToggleComponent
    {
        [SerializeField] TextMeshProUGUI AmountField;

        protected Booster Booster;

        protected string AmountText
        {
            set
            {
                if (AmountField.text == value)
                    return;
                
                AmountField.text = value;

                AmountField.DOKill();
                AmountField.transform.localScale = Vector3.one;
                AmountField.transform.parent.
                    DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.4f, 2, 0.7f);
            }
        }
        
        public void Initialize(Booster booster)
        {
            Booster = booster;
            
            IsOn = Booster.IsActivated;
            AmountText = Booster.Amount.ToString();
            
            ToggleValueChanged += value =>
            {
                if (value && Booster.Amount <= 0)
                {
                    CreateTierWindow();
                    IsOn = false;
                    return;
                }

                if (value)
                    Booster.Activate();
                else
                    Booster.Deactivate();
            };

            Booster.AmountChangedEvent += () =>
            {
                AmountText = Booster.Amount.ToString();
            };
            
            Booster.BoosterActivatedEvent += () => IsOn = true;
            Booster.BoosterDeactivatedEvent += () => IsOn = false;

        }

        void CreateTierWindow()
        {
            Tier boosterTier = Account.Tiers.FirstOrDefault(tier =>
                tier.Type == Tier.TierType.Booster && tier.Reward is BoosterReward boosterReward &&
                boosterReward.Booster == Booster);

            if (boosterTier == null)
                return;
            
            TierWindow.Create(boosterTier.ID, () => IsOn = true);
        }
    }
}