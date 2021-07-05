
using Abu.Tools.UI;
using UnityEngine;

public abstract class Reward
{
    public abstract Rarity Rarity { get; }
    public abstract UIComponent CreateView(RectTransform container); 
    public abstract void Claim();
}
