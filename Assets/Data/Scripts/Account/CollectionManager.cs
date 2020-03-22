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
    
    private void Start()
    {
        CollectionData.LoadSettings();
    }

    private void OnApplicationQuit()
    {
        CollectionData.SaveSettings();
    }
}
