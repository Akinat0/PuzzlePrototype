using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private CollectionData CollectionData;

    public CollectionItem[] CollectionItems => CollectionData.CollectionItems;

    public int DefaultItemID {
        get => CollectionData.DefaultItemID;
        set
        {
            if (CollectionData == null || CollectionData.DefaultItemID == value)
                return;
            
            CollectionData.DefaultItemID = value;
        }
    }

    public bool UnlockItem(int ID)
    {
        return CollectionData.UnlockItem(ID);
    }

    public void UnlockItemColor(int ID, string colorID)
    {
        CollectionData.UnlockItemColor(ID, colorID);
    }

    public CollectionItem GetCollectionItem(int ID)
    {
        return CollectionData.GetCollectionItem(ID);
    }
    
    public CollectionItem GetCollectionItem(string itemName)
    {
        return CollectionData.GetCollectionItem(itemName);
    }
    
    
    public CollectionItem DefaultItem => CollectionData.DefaultItem;
    
    void Awake()
    {
        CollectionData.LoadSettings();
    }

    void OnDestroy()
    {
        CollectionData.SaveSettings();
    }
    
}
