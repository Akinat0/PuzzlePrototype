using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelRootView : MonoBehaviour
{
    [SerializeField] private PlayerView m_PlayerView; 
    [SerializeField] private BackgroundView m_BackgroundView;

    Renderer[] renderers;

    Renderer[] Renderers
    {
        get
        {
            if (renderers == null)
                renderers = GetComponentsInChildren<Renderer>();

            if(renderers.Any(r => r == null))
                renderers = GetComponentsInChildren<Renderer>();
            
            return renderers;
        }
    }

    SortingGroup sortingGroup;

    void Awake()
    {
        sortingGroup = gameObject.AddComponent<SortingGroup>();
    }
    
    public void SetActiveLevelRoot(bool value)
    {
        foreach (Renderer renderer in Renderers)
            renderer.enabled = value;
    }

    public PlayerView PlayerView
    {
        get => m_PlayerView;
        set
        {
            m_PlayerView = value;
            m_PlayerView.transform.SetParent(transform);
            m_PlayerView.transform.localPosition = Vector3.zero;
        }
    }

    public void SetSortingPriorityHigh()
    {
        sortingGroup.sortingOrder = 1;
    }
    
    public void SetSortingPriorityLow()
    {
        sortingGroup.sortingOrder = 0;
    }
    public BackgroundView BackgroundView => m_BackgroundView;

}
