using System;

public class TutorialPropertyAttribute : Attribute
{
    public string Key { get; }

    public TutorialPropertyAttribute(string key)
    {
        Key = key;
    }
}
