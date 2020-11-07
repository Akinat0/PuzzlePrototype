
using UnityEngine;

public abstract class Reward
{
    public abstract GameObject CreateView(RectTransform container); 
    public abstract void Claim();
}
