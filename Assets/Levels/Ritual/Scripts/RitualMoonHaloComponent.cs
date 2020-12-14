using System.Collections.Generic;
using PuzzleScripts;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class RitualMoonHaloComponent : MonoBehaviour
{
    [SerializeField] CustomBlend HaloBlend;
    [SerializeField] SpriteRenderer HaloRays;
    
    [SerializeField, Range(0, 1)] float MinFadeValue = 0.1f;
    
    float CurrentHaloFade;

    new Collider2D collider;
    Collider2D Collider
    {
        get
        {
            if (collider == null)
                collider = GetComponent<Collider2D>();
            
            return collider;
        }
    }

    List<EnemyBase> Enemies;
    
    void OnCollisionStay2D(Collision2D other)
    {
        Debug.LogError("Object " + other.transform.name + " within halo");
        
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();

        if(enemy == null)
            return;

        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();

        if (enemyCollider == null)
        {
            Debug.LogWarning("Enemy doesn't contain collider");
            return;
        }

        float intersection = BoundsContainedPercentage(enemyCollider.bounds, Collider.bounds);
        
        float fade = intersection.Remap(0, 1, MinFadeValue, 1);

        CurrentHaloFade = Mathf.Min(fade, CurrentHaloFade);
    }

    void OnWillRenderObject()
    {
        float h, s, v;
        Color.RGBToHSV(HaloBlend.TextureColor, out h, out s, out v);
        v *= CurrentHaloFade;
        HaloBlend.TextureColor = Color.HSVToRGB(h, s, v);

        Color raysColor = HaloRays.color;
        raysColor.a *= CurrentHaloFade;
        HaloRays.color = raysColor;

        CurrentHaloFade = 1;
    }

    private float BoundsContainedPercentage(Bounds other, Bounds region )
    {

        return 0;
    }
    
}
