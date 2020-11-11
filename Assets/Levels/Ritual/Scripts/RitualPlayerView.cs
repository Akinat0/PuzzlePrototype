using PuzzleScripts;
using UnityEngine;

public class RitualPlayerView : SkinPlayerView
{
    [SerializeField] SkinContainer GlyphSkin;

    protected override void OnPlayerLoseHp()
    {
        //GlyphSkin.Skin = (GlyphSkin.Skin + 1) % GlyphSkin.Length;
        
        base.OnPlayerLoseHp();
    }

    protected override void OnEnemyDied(EnemyBase enemy)
    {
        //GlyphSkin.Skin = (GlyphSkin.Skin + 1) % GlyphSkin.Length;
        
        base.OnEnemyDied(enemy);
    }
}
