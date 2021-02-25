using System.Linq;
using UnityEngine;

public abstract class PlayerViewSkin : MonoBehaviour
{
    public abstract void ChangePuzzleSkin(PuzzleColorData puzzleColor);

#if UNITY_EDITOR
    
    protected PuzzleColorData[] GetPuzzleColors()
    {
        string path = UnityEditor.PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
        string puzzleName = path.Split('/')[2];
        CollectionData data = UnityEditor.AssetDatabase.LoadAssetAtPath<CollectionData>("Assets/Data/Account/CollectionData.asset");
        
        CollectionItem collectionItem = data.CollectionItems.FirstOrDefault(item => item.Name == puzzleName);

        if (collectionItem == null)
        {
            Debug.LogError($"Collection item {puzzleName} doesn't exist");
            return null;
        }

        return collectionItem.puzzleColors;
    } 

#endif
}
