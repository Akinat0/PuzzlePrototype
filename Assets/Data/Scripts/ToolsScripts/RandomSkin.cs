using UnityEngine;

public class RandomSkin : SkinContainerBase
{
    protected override void Start()
    {
        base.Start();
        SetRandomSkin();
    }

    public void SetRandomSkin()
    {
        index = Random.Range(0, _Sprites.Length);
        _SpriteRenderer.sprite = _Sprites[index];
    }

}
