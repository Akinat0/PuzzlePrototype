using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class RequireTypeAttribute : PropertyAttribute
{
    public readonly Type Type;

    public RequireTypeAttribute(Type _Type)
    {
        Type = _Type;
    }
}
