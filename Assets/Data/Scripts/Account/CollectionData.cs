using UnityEngine;
[CreateAssetMenu(fileName = "new CollectionData", menuName = "Account/CollectionData", order = 51)]
public class CollectionData : SaveableScriptableObject
{
    [SerializeField] private CollectionItem[] _CollectionItems;
    [SerializeField] public int DefaultItemID;
    

    public CollectionItem DefaultItem => _CollectionItems[DefaultItemID];
    public CollectionItem[] CollectionItems => _CollectionItems;
    
    public override void LoadSettings()
    {
        base.LoadSettings();
        Debug.Log($"Collection was loaded. Default item is {DefaultItemID}, his name is {DefaultItem.name}");
    }
    
    public override void SaveSettings()
    {
        base.SaveSettings();
        Debug.Log($"Collection was saved. Default item is {DefaultItemID}, his name is {DefaultItem.name}");
    }
}