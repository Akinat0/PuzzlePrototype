using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class FruitsVirus : VirusEnemy
{
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite[] Skins;
    [SerializeField] GameObject[] Effects;
    
    public override Renderer Renderer => spriteRenderer;
    int SkinID { get; set; }
        
    public override void Instantiate(EnemyParams @params)
    {
        base.Instantiate(@params);

        SkinID = Random.Range(0, Mathf.Min(Skins.Length, Effects.Length));
        spriteRenderer.sprite = Skins[SkinID];
        vfx = Effects[SkinID];
    }

    public override Transform Die()
    {
        Transform vfxTransform = base.Die();
        vfxTransform.rotation = spriteRenderer.transform.rotation;
        return vfxTransform;
    }
}
