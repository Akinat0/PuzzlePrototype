using Abu.Tools;
using UnityEngine;

public class DawnWaterReflection : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] SpriteRenderer reflectionArea;
    void Start()
    {
        ScreenScaler.FocusCameraOnBounds(reflectionArea.bounds, camera);
        camera.transform.SetZ(0);
    }
}
