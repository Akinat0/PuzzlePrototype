using UnityEngine;
using UnityEngine.Rendering;

public class LevelRootView : MonoBehaviour
{
    [SerializeField] private PlayerView m_PlayerView; 
    [SerializeField] private BackgroundView m_BackgroundView;

    Renderer[] renderers;
    Renderer[] Renderers => renderers ?? (renderers = GetComponentsInChildren<Renderer>());

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
        get { return m_PlayerView;}
        set
        {
            m_PlayerView = value;
            m_PlayerView.transform.SetParent(transform);
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
