
using UnityEngine;

public enum Rarity
{
    None   = 0,
    Common = 1,
    Rare   = 2,
    Epic   = 3
}

public static class RarityExtension 
{
    public static Color GetColor(this Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return new Color(0.679f, 0.679f, 0.679f);
            case Rarity.Rare:
                return new Color(0.287f, 0.843f, 1f);
            case Rarity.Epic:
                return new Color(0.988f, 0.485f, 1f);
            default:
                return Color.gray;
        }
    }
}
