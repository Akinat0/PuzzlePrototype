using System;
using Abu.Tools;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))] 
public class BubbleDialog : MonoBehaviour
{
    #region constants
    
    const string path = "Prefabs/BubbleDialog";

    #endregion

    #region factory
   
    public static BubbleDialog Create(PlayerView playerView)
    {
        BubbleDialog bubbleDialog = Instantiate(Resources.Load<GameObject>(path)).GetComponent<BubbleDialog>();

        Transform dialogTransform = bubbleDialog.transform;
        dialogTransform.parent = playerView.transform;
        dialogTransform.localScale =
            ScreenScaler.FitHorizontalPart(bubbleDialog.Background, 0.35f) *
            Vector2.one;
                    
        dialogTransform.localScale = playerView.transform.InverseTransformPoint(dialogTransform.localScale);
                    
        //Put dialog on the top right puzzle angle
        float halfOfPuzzleWidth = ScreenScaler.PartOfScreen(playerView.PartOfScreen / 2).x;
        dialogTransform.position = Vector2.one * halfOfPuzzleWidth;
        
        return bubbleDialog;
    }
    
    #endregion
    
    #region attributes

    static readonly int ShowID = Animator.StringToHash("Show");
    
    SpriteRenderer Background => background;
    public bool Shown { get; private set; }

    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] SpriteRenderer background;

    AnimationEventBehaviour hideBehaviour;
    AnimationEventBehaviour showBehaviour;
    Animator animator;
    
    Action OnShow;
    Action OnHide;
    
    #endregion
    
    #region public
    public void Show(string message, Action finish = null)
    {
        if (Shown)
        {
            finish?.Invoke();
            return;
        }

        messageText.text = message;
        OnShow += finish;
        animator.SetBool(ShowID, true);
    }
    
    public void Hide(Action finish = null)
    {
        if (!Shown)
        {
            finish?.Invoke();
            return;
        }

        OnHide += finish;
        animator.SetBool(ShowID, false);
    }
    
    #endregion

    #region private

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        showBehaviour = AnimationEventBehaviour.FindState(animator, "show");
        hideBehaviour = AnimationEventBehaviour.FindState(animator, "hide");

        showBehaviour.OnComplete += () => Shown = true;
        hideBehaviour.OnComplete += () => Shown = false;
        
        showBehaviour.OnComplete += InvokeOnShowAction;
        hideBehaviour.OnComplete += InvokeOnHideAction;
    }
    void InvokeOnHideAction()
    {
        Action action = OnHide;
        OnHide = null;
        action?.Invoke();
    }
    
    void InvokeOnShowAction()
    {
        Action action = OnShow;
        OnShow = null;
        action?.Invoke();
    }
    
    #endregion
}
