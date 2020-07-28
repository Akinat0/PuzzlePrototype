
using UnityEngine.UI;

public class AchievementViewComponent : TextButtonComponent
{
     Slider progressBar;

     private Slider ProgressBar
     {
          get
          {
               if (progressBar == null)
                    progressBar = GetComponentInChildren<Slider>();
               
               return progressBar;
          }
     }

     public void SetupProgress(float value, float minValue, float maxValue)
     {
          ProgressBar.minValue = minValue;
          ProgressBar.maxValue = maxValue;
          ProgressBar.value = value;
     }
}
