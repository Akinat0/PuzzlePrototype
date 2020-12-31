
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))] 
public class BubbleDialog : MonoBehaviour
{
    private const string path = "Prefabs/BubbleDialog";
    private const string hideBehaviourID = "hide";
    
    [SerializeField] private Text messageText;
    [SerializeField] private SpriteRenderer background;

    private AnimationEventBehaviour hideBehaviour;

    private Animator animator;
    
    private static readonly int SHOW = Animator.StringToHash("Show");

    public SpriteRenderer Background => background;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        hideBehaviour = AnimationEventBehaviour.FindState(animator, hideBehaviourID);

        hideBehaviour.OnExit += () =>
        {
            if(gameObject != null)
                Destroy(gameObject);
        };
    }

    public void SetRenderLayer(string layer, int? layerOrder = null)
    {
        background.sortingLayerName = layer;
        messageText.canvas.sortingLayerName = layer;

        if (layerOrder != null)
        {
            background.sortingOrder = layerOrder.Value;
            messageText.canvas.sortingOrder = layerOrder.Value;
        }
    }
    
    public void Show(string message)
    {
        messageText.text = message;
        animator.SetBool(SHOW, true);
    }
    
    public void Hide(Action onFinish = null)
    {
        animator.SetBool(SHOW, false);

        if (onFinish != null)
            hideBehaviour.OnExit += onFinish;
    }

    public static BubbleDialog Create(Action<BubbleDialog> scaleRules = null)
    {
        BubbleDialog bubbleDialog = Instantiate(Resources.Load<GameObject>(path)).GetComponent<BubbleDialog>();

        scaleRules?.Invoke(bubbleDialog);

        return bubbleDialog;
    }
}
