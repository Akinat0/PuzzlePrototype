using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private CollectionData CollectionData;

    public CollectionItem[] CollectionItems => CollectionData.CollectionItems;

    public int DefaultItemID {
        get { return CollectionData.DefaultItemID; }
        set { CollectionData.DefaultItemID = value; }
    }

    public CollectionItem DefaultItem => CollectionData.DefaultItem;
    
    void Start()
    {
        CollectionData.LoadSettings();
    }

    void OnApplicationQuit()
    {
        CollectionData.SaveSettings();
    }
}
