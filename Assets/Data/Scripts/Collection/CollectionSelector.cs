using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using UnityEngine;

public class CollectionSelector : SelectorBase
{
    [SerializeField] private CollectionItem[] Selection;
    [SerializeField] private Transform ItemContainer;
    protected override int Length => Selection.Length;

    protected override void CreateItem(int _Index)
    {
        Instantiate(Selection[_Index], ItemContainer);
    }
}
