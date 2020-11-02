using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "new CollectionData", menuName = "Account/CollectionData", order = 51)]
public class CollectionData : ScriptableObject
{
    [SerializeField] CollectionItem[] _CollectionItems;
    
    int defaultItemID;
    
    #region public
    
    public CollectionItem DefaultItem => _CollectionItems[defaultItemID];
    public CollectionItem[] CollectionItems => _CollectionItems;

    public int DefaultItemID
    {
        get => defaultItemID;
        set
        {
            if (value == defaultItemID)
                return;

            defaultItemID = value;
            SaveDefaultItem();
        }
    }

    public bool UnlockItem(int ID)
    {
        CollectionItem item = GetCollectionItem(ID);

        if (item == null)
            return false;

        if (item.DefaultUnlocked || item.Unlocked)
            return false;

        item.Unlocked = true;

        SaveItem(ID);
        
        return true;
    }

    public CollectionItem GetCollectionItem(int ID)
    {
        return CollectionItems.FirstOrDefault(item => item.ID == ID);
    }
    
    public CollectionItem GetCollectionItem(string itemName)
    {
        return CollectionItems.FirstOrDefault(item => item.Name == itemName);
    }
    
    public void LoadSettings()
    {
        LoadItems();
        LoadDefaultItem();
    }

    public void SaveSettings()
    {
        SaveItems();
        SaveDefaultItem();
    }

    #endregion

    #region private 
    
    string DefaultItemIDKey => "default_item_key";
    
    string GetItemUnlockedKey(int ID)
    {
        return $"collection_item_unlocked_key_{ID}";
    }

    void LoadItems()
    {
        foreach (CollectionItem item in CollectionItems)
        {
            int isUnlocked = PlayerPrefs.GetInt(GetItemUnlockedKey(item.ID), item.DefaultUnlocked ? 1 : 0);
            item.Unlocked = isUnlocked == 1;
        }
    }

    void LoadDefaultItem()
    {
        defaultItemID = PlayerPrefs.GetInt(DefaultItemIDKey, 0);
    }
    
    void SaveItem(int ID)
    {
        CollectionItem item = GetCollectionItem(ID);
        
        if(item == null)
            return;

        PlayerPrefs.SetInt(GetItemUnlockedKey(ID), item.Unlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    void SaveItems()
    {
        foreach (CollectionItem item in CollectionItems)
            PlayerPrefs.SetInt(GetItemUnlockedKey(item.ID), item.Unlocked ? 1 : 0);
        
        PlayerPrefs.Save();
    }
    
    void SaveDefaultItem()
    {
        PlayerPrefs.SetInt(DefaultItemIDKey, defaultItemID);
        PlayerPrefs.Save();
        Debug.Log($"Default item saved. It is {defaultItemID}, his name is {DefaultItem.name}");
    }
    
    #endregion
}