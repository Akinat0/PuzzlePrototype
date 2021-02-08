using UnityEngine;

public class SpriteGradient : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] SpriteRenderer[] targetSprites;

    public Gradient Gradient
    {
        get => gradient;
        set
        {
            if(gradient.Equals(value))
                return;
            
            gradient = value;
            UpdateGradient();
        }
    }

    void UpdateGradient()
    {
        for (int i = 0; i < targetSprites.Length; i++)
        {
            SpriteRenderer sprite = targetSprites[i];
            sprite.color = Gradient.Evaluate((float) i / targetSprites.Length);
        }
    }
    
    void OnValidate()
    {
        UpdateGradient();
    }

    void OnDidApplyAnimationProperties()
    {
        UpdateGradient();
    }
}
