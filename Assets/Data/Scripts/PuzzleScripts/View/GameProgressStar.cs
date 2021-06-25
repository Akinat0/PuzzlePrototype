using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    public class GameProgressStar : MonoBehaviour
    {
        static readonly int ActiveID = Animator.StringToHash("Active");

        [SerializeField] Animator animator;
        [SerializeField] Image image;
        
        RectTransform rectTransform;
        public RectTransform RectTransform =>
            rectTransform ? rectTransform : rectTransform = transform as RectTransform;

        bool isActive;
        public bool IsActive
        {
            get => isActive;
            set
            {
                if(isActive == value)
                    return;

                image.color = Color.white;
                isActive = value;
                
                animator.SetBool(ActiveID, isActive);
            }
        }
        
        public float Fill
        {
            get => image.fillAmount;
            set => image.fillAmount = value;
        }
        
    }
}