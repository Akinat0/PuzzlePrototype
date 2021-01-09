using System;
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

    public void UnlockItemColor(int ID, string colorID)
    {
        CollectionItem item = GetCollectionItem(ID);

        if (item == null)
            return;

        PuzzleColorData puzzleColorData = item.PuzzleColors.FirstOrDefault(colorData => colorData.ID == colorID);
        
        int colorIndex = Array.IndexOf(item.PuzzleColors, puzzleColorData);
        puzzleColorData.IsUnlocked = true;
        item.PuzzleColors[colorIndex] = puzzleColorData;
        
        SaveItem(ID);
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

    string GetItemColorIndexKey(int ID)
    {
        return $"collection_item_color_index_{ID}";
    }

    string GetItemColorUnlocked(int ID, string colorID)
    {
        return $"collection_item_{ID}_color_unlocked_{colorID}";
    }

    void LoadItems()
    {
        foreach (CollectionItem item in CollectionItems)
        {
            int isUnlocked = PlayerPrefs.GetInt(GetItemUnlockedKey(item.ID), item.DefaultUnlocked ? 1 : 0);
            item.Unlocked = isUnlocked == 1;
            
            int colorIndex = PlayerPrefs.GetInt(GetItemColorIndexKey(item.ID), 0);
            item.ActiveColorIndex = colorIndex;

            for (int i = 0; i < item.PuzzleColors.Length; i++)
                item.PuzzleColors[i] = LoadPuzzleColorData(item.ID, item.PuzzleColors[i]);
        }
    }

    void LoadDefaultItem()
    {
        defaultItemID = PlayerPrefs.GetInt(DefaultItemIDKey, 0);
    }

    PuzzleColorData LoadPuzzleColorData(int ID, PuzzleColorData colorData)
    {
        int unlocked = PlayerPrefs.GetInt(GetItemColorUnlocked(ID, colorData.ID), colorData.DefaultUnlocked ? 1 : 0);
        colorData.IsUnlocked = unlocked == 1;
        return colorData;
    }

    void SaveItem(int ID)
    {
        CollectionItem item = GetCollectionItem(ID);
        
        if(item == null)
            return;

        PlayerPrefs.SetInt(GetItemUnlockedKey(ID), item.Unlocked ? 1 : 0);
        PlayerPrefs.SetInt(GetItemColorIndexKey(ID), item.ActiveColorIndex);
        
        foreach (PuzzleColorData puzzleColor in item.PuzzleColors)
            PlayerPrefs.SetInt(GetItemColorUnlocked(ID, puzzleColor.ID), puzzleColor.IsUnlocked ? 1 : 0);    
        
        PlayerPrefs.Save();
    }
    
    void SaveItems()
    {
        foreach (CollectionItem item in CollectionItems)
        {
            PlayerPrefs.SetInt(GetItemUnlockedKey(item.ID), item.Unlocked ? 1 : 0);
            PlayerPrefs.SetInt(GetItemColorIndexKey(item.ID), item.ActiveColorIndex);
            
            foreach (PuzzleColorData puzzleColor in item.PuzzleColors)
                PlayerPrefs.SetInt(GetItemColorUnlocked(item.ID, puzzleColor.ID), puzzleColor.IsUnlocked ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    void SaveDefaultItem()
    {
        PlayerPrefs.SetInt(DefaultItemIDKey, defaultItemID);
        PlayerPrefs.Save();
        Debug.Log($"Default item saved. It is {defaultItemID.ToString()}, his name is {DefaultItem.name}");
    }
    
    #endregion
}