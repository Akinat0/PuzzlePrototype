using System;
using System.Collections;
using Data.Scripts.Tools.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Abu.Tools.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Button), typeof(UIScaleComponent))]
    public class ButtonComponent : UIComponent, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected AudioClip Sound;
        [SerializeField] protected Color ButtonColor = Color.white;
        [SerializeField] protected Color AlternativeButtonColor = Color.white;
        [SerializeField] protected ImageComponent icon;
        [SerializeField] protected Haptic.Type hapticType = Haptic.Type.SELECTION;

        public event Action OnClick;
        public event Action OnHoldDown;
        public event Action OnHoldUp;
        public event Action OnHoldEnter;
        public event Action OnHoldExit;
        
        public virtual bool Interactable
        {
            get => Button.interactable;
            set => Button.interactable = value;
        }
        
        public virtual UIScaleComponent ScaleComponent
        {
            get
            {
                if(scaleComponent == null)
                    scaleComponent = GetComponent<UIScaleComponent>();
                return scaleComponent;
            }
        }
        
        public virtual Color Color
        {
            get => ButtonColor;
            set
            {
                if (ButtonColor == value)
                    return;

                ButtonColor = value;
                
                if (ColorProvider != null)
                    ColorProvider.ApplyColor(Color, AlternativeColor);
            }
        }

        public virtual Color AlternativeColor
        {
            get => AlternativeButtonColor;
            set
            {
                if(AlternativeButtonColor == value)
                    return;

                AlternativeButtonColor = value;

                if (ColorProvider != null)
                    ColorProvider.ApplyColor(Color, AlternativeColor);
            }
        }

        public UIColorProvider ColorProvider
        {
            get
            {
                if (colorProvider == null)
                    colorProvider = GetComponent<UIColorProvider>();

                return colorProvider;
            }
        }

        public virtual Image Background
        {
            get
            {
                if (background == null)
                    background = GetComponent<Image>();
                
                return background;
            }
        }


        protected virtual Button Button
        {
            get
            {
                if (button == null)
                    button = GetComponent<Button>();
                
                return button;
            }
        }

        public ImageComponent Icon => icon;

        UIColorProvider colorProvider;
        UIScaleComponent scaleComponent;
        Image background;
        Button button;

        IEnumerator currentScaleRoutine;

        public void OnButtonClick()
        {
            //Interactable condition is already into account
            if (MobileInput.Condition)
            {
                OnClick?.Invoke();
                
                if(Sound != null && SoundManager.Instance != null)
                    SoundManager.Instance.PlayOneShot(Sound);
                
                Haptic.Run(hapticType);
            }
        }

        public virtual void ShowComponent(float duration = 0.2f, Action finished = null)
        {
            SetActive(true);
            
            if (!gameObject.activeInHierarchy)
            {
                finished?.Invoke();
                return;
            }
            
            if(currentScaleRoutine != null)
                StopCoroutine(currentScaleRoutine);

            StartCoroutine(currentScaleRoutine = ScaleRoutine(1, duration, finished));
        }

        public virtual void HideComponent(float duration = 0.2f, Action finished = null)
        {
            if (!gameObject.activeInHierarchy)
            {
                finished?.Invoke();
                return;
            }

            Interactable = false;
            if (currentScaleRoutine != null)
                StopCoroutine(currentScaleRoutine);

            StartCoroutine(currentScaleRoutine = ScaleRoutine(0, duration, () =>
            {
                SetActive(false);
                Interactable = true;
                finished?.Invoke();
            }));
        }

        IEnumerator ScaleRoutine(float targetScale, float duration, Action finished = null)
        {
            float sourceScale = ScaleComponent.Phase;
            float time = 0;
            
            while (time < duration)
            {
                ScaleComponent.Phase = Mathf.Lerp(sourceScale, targetScale, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            ScaleComponent.Phase = targetScale;
            
            finished?.Invoke();
        }
        
        
        protected override void OnValidate()
        {
            if (ColorProvider != null)
                ColorProvider.ApplyColor(Color, AlternativeColor);
        }

        protected virtual void OnDidApplyAnimationProperties()
        {
            if (ColorProvider != null)
                ColorProvider.ApplyColor(Color, AlternativeColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnHoldDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnHoldUp?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoldEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoldExit?.Invoke();
        }

        #if UNITY_EDITOR
        [ContextMenu("Show")]
        public void ShowEditor()
        {
            ShowComponent(finished:() => Debug.LogError("Shown"));
        }
        
        [ContextMenu("Hide")]
        public void HideEditor()
        {
            HideComponent(finished:() => Debug.LogError("Hidden"));
        }
        #endif
    }
}