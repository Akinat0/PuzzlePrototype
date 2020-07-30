using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using Puzzle;
using UnityEngine;

public class SetPuzzlesForCamera : MonoBehaviour
{
    [SerializeField] CollectionItem[] items;

    readonly Dictionary<string, Transform> activeItems = new Dictionary<string, Transform>();

    Camera renderCamera;
    //TODO move all to local position and set objects as children of renderCamera
    void Awake()
    {
        Debug.LogError("Camera size " + ScreenScaler.CameraSize);
        
        renderCamera = GetComponent<Camera>();

        for (int i = 0; i < items.Length; i++)
        {
            CollectionItem item = items[i];
            Transform puzzle = Instantiate(item.GetPuzzleVariant(new PuzzleSides(true, true, true, true))).transform;

            int column = i % 4;
            int row = i / 4;
            Debug.LogError("ROW " + row);
            float xPos = - ScreenScaler.CameraSize.x / 2.0f + ScreenScaler.CameraSize.x / 4.0f * column + ScreenScaler.CameraSize.x / 8f;
            float yPos = ScreenScaler.CameraSize.y / 2.0f - ScreenScaler.CameraSize.x / 4.0f  * row - ScreenScaler.CameraSize.x / 8f;
            
            Vector3 position = new Vector3(xPos, yPos);

            puzzle.position = position;

            activeItems[item.Name] = puzzle;
        }

    }

    public Rect GetPuzzleUV(string itemName)
    {
        if(!activeItems.ContainsKey(itemName))
            return Rect.zero;

        Vector2 halfCameraSize = ScreenScaler.CameraSize / 2f;
        
        //To camera coordinates
        Vector3 position = activeItems[itemName].position + new Vector3(halfCameraSize.x, halfCameraSize.y);

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

        Debug.LogError($"Item {itemName} row {row} column {column}");
        
        float xPos = column * 0.25f;
        float yPos = 1 - (row + 1) * ScreenScaler.CameraSize.x / ScreenScaler.CameraSize.y * 0.25f;
        float width = 0.25f;
        float height = ScreenScaler.CameraSize.x / ScreenScaler.CameraSize.y * 0.25f;

        return new Rect(xPos, yPos, width, height);
    }

}
