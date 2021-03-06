using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

public class AchievementViewComponent : TextButtonComponent
{
     [SerializeField] protected TextComponent Description;
     [SerializeField] public RectTransform RewardContainer;
     [SerializeField] protected Image iconImage;
     
     Slider progressBar;

     public TextComponent DescriptionField => Description;

     public string DescriptionText
     {
          get => Description.Text;
          set => Description.Text = value;
     }

     public void CreateReward(Reward reward)
     {
          reward.CreateView(RewardContainer);               
     }
     
     public void SetupProgressBar(float value, float minValue, float maxValue)
     {
          ProgressBar.minValue = minValue;
          ProgressBar.maxValue = maxValue;
          ProgressBar.value = value;
     }

     public Sprite Icon
     {
          set => iconImage.sprite = value;
     }
     
     Slider ProgressBar
     {
          get
          {
               if (progressBar == null)
                    progressBar = GetComponentInChildren<Slider>();
               
               return progressBar;
          }
     }

     
     
}
