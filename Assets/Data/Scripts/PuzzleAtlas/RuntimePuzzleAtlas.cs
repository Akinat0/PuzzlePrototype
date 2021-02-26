using System;
using System.Collections.Generic;
using Abu.Tools;
using UnityEngine;

public class RuntimePuzzleAtlas : MonoBehaviour
{
    public static RuntimePuzzleAtlas Instance;
    
    readonly Dictionary<int, Transform> inactiveItems = new Dictionary<int, Transform>();
    readonly Dictionary<int, Transform> activeItems = new Dictionary<int, Transform>();

    public event Action RebuildPuzzlesAtlas;

    Camera renderCamera;
    RenderTexture atlasTexture;

    Vector2 ScreenSize;
    Vector2 CameraSize;
    
    
    void Awake()
    {
        Instance = this;

        ScreenSize = ScreenScaler.ScreenSize;
        CameraSize = ScreenScaler.CameraSize;
        
        atlasTexture = Resources.Load<RenderTexture>("Collection/CollectionRenderTexture");
        atlasTexture.width = Mathf.CeilToInt(ScreenSize.x);
        atlasTexture.height = Mathf.CeilToInt(ScreenSize.y);
        
        renderCamera = GetComponent<Camera>();
        renderCamera.rect = new Rect(0, 0, 1, ScreenSize.y / ScreenSize.x );

        foreach (var item in Account.CollectionItems)
        {
            inactiveItems[item.ID] = PlayerView.Create(transform, item.ID).transform;
            inactiveItems[item.ID].localPosition = CameraSize * 3;
        }
        
        gameObject.SetActive(false);
    }

    public Rect GetPuzzleRectInAtlas(int id)
    {
        if (!activeItems.ContainsKey(id))
            RequestItem(id);

        return GetItemUV(id);
    }
    
    public void DeactivateItem(int id)
    {
        if (activeItems.ContainsKey(id))
        {
            inactiveItems[id] = activeItems[id];
            activeItems.Remove(id);
        }

        if (inactiveItems[id] == null)
            return;
        
        //Set invalid position
        inactiveItems[id].position = ScreenSize * 3; 
        
        CreateActiveItems();
    }
    
    
    void RequestItem(int id)
    {
        if (activeItems.ContainsKey(id))
            return;

        activeItems[id] = inactiveItems[id];
        inactiveItems.Remove(id);
        
        CreateActiveItems();
    }

    void CreateActiveItems()
    {
        if (activeItems.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        int i = 0;

        foreach (int key in activeItems.Keys)
        {
            Transform puzzle = activeItems[key];

            int column = i % 4;
            int row = i / 4;
            
            float xPos = - CameraSize.x / 2.0f + CameraSize.x / 4.0f * column + CameraSize.x / 8f;
            float yPos = CameraSize.y / 2.0f - CameraSize.x / 4.0f  * row - CameraSize.x / 8f;
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

        Vector2 halfCameraSize = CameraSize / 2f;
        
        //To camera coordinates
        Vector3 position = activeItems[id].localPosition + new Vector3(halfCameraSize.x, halfCameraSize.y);

        int column = 0;
        int row = 0;
        
        float step = CameraSize.x / 4;
            
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
        
        minRange = CameraSize.y - step;
        maxRange = CameraSize.y;
        
        while (!(position.y >= minRange && position.y <= maxRange || counter >= 100))
        {
            counter++;
            row++;
            minRange -= step;
            maxRange -= step;
        }
        
        float xPos = column * 0.25f;
        float yPos = 1 - (row + 1) * CameraSize.x / CameraSize.y * 0.25f;
        float width = 0.25f;
        float height = CameraSize.x / CameraSize.y * 0.25f;

        return new Rect(xPos, yPos, width, height);
    }

}
