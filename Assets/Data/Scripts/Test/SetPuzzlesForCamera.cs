using System;
using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using Puzzle;
using UnityEngine;

public class SetPuzzlesForCamera : MonoBehaviour
{
    [SerializeField] CollectionItem[] items;

    readonly Dictionary<int, Transform> inactiveItems = new Dictionary<int, Transform>();
    readonly Dictionary<int, Transform> activeItems = new Dictionary<int, Transform>();

    public event Action RebuildPuzzlesAtlas;

    Camera renderCamera;
    
    void Awake()
    {
        renderCamera = GetComponent<Camera>();

        foreach (var item in items)
        {
            inactiveItems[item.ID] = Instantiate(item.GetAnyPuzzleVariant(), transform).transform;
            inactiveItems[item.ID].localPosition = ScreenScaler.CameraSize * 3;
        }
    }

    public Rect GetPuzzleRectInAtlas(int id)
    {
        if (!activeItems.ContainsKey(id))
            RequestItem(id);
        
        return GetItemUV(id);
    }
    
    public void DeactivateItem(int id)
    {
        Debug.Log($"Deactivate {id}");
        
        if (activeItems.ContainsKey(id))
        {
            inactiveItems[id] = activeItems[id];
            activeItems.Remove(id);
        }

        if (inactiveItems[id] == null)
            return;
        
        //Set invalid position
        inactiveItems[id].position = ScreenScaler.ScreenSize * 3; 
        
        CreateActiveItems();
    }
    
    void RequestItem(int id)
    {
        if (activeItems.ContainsKey(id))
            return;

        Debug.Log($"Activate {id}");
        
        activeItems[id] = inactiveItems[id];
        inactiveItems.Remove(id);
        
        CreateActiveItems();
    }

    void CreateActiveItems()
    {
        int i = 0;
        
        foreach (int key in activeItems.Keys)
        {
            Transform puzzle = activeItems[key];

            int column = i % 4;
            int row = i / 4;
            
            float xPos = - ScreenScaler.CameraSize.x / 2.0f + ScreenScaler.CameraSize.x / 4.0f * column + ScreenScaler.CameraSize.x / 8f;
            float yPos = ScreenScaler.CameraSize.y / 2.0f - ScreenScaler.CameraSize.x / 4.0f  * row - ScreenScaler.CameraSize.x / 8f;
            float zPos = 10;
            
            Vector3 position = new Vector3(xPos, yPos, zPos);

            puzzle.localPosition = position;

            i++;
        }
        
        RebuildPuzzlesAtlas?.Invoke();
    }

    Rect GetItemUV(int id)
    {
        if(!activeItems.ContainsKey(id))
            return Rect.zero;

        Vector2 halfCameraSize = ScreenScaler.CameraSize / 2f;
        
        //To camera coordinates
        Vector3 position = activeItems[id].localPosition + new Vector3(halfCameraSize.x, halfCameraSize.y);

        int column = 0;
        int row = 0;
        
        float step = ScreenScaler.CameraSize.x / 4;
            
        float minRange = 0;
        float maxRange = step;

        int counter = 0;
        
        while (!(position.x >= minRange && position.x <= maxRange || counter >= 100))
        {
            counter++;
            column++;
            minRange += step;
            maxRange += step;
        }
        
        minRange = ScreenScaler.CameraSize.y - step;
        maxRange = ScreenScaler.CameraSize.y;
        
        while (!(position.y >= minRange && position.y <= maxRange || counter >= 100))
        {
            counter++;
            row++;
            minRange -= step;
            maxRange -= step;
        }
        
        float xPos = column * 0.25f;
        float yPos = 1 - (row + 1) * ScreenScaler.CameraSize.x / ScreenScaler.CameraSize.y * 0.25f;
        float width = 0.25f;
        float height = ScreenScaler.CameraSize.x / ScreenScaler.CameraSize.y * 0.25f;

        return new Rect(xPos, yPos, width, height);
    }

}
