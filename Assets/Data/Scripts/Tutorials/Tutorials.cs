using System;
using System.Collections.Generic;
using System.Reflection;
using ScreensScripts;
using UnityEngine;

public static partial class Tutorials
{
    static readonly Dictionary<string, object> Registry = new Dictionary<string, object>();

    public static void Register(string key, object entity)
    {
        if (Registry.ContainsKey(key))
        {
            Debug.LogWarning($"Tutorial registry already contains key {key} and object of type {Registry[key].GetType()} it will be overriden");
            Registry[key] = entity;
            return;
        }
        
        Registry.Add(key, entity);
    }

    public static void Unregister(string key)
    {
        if (!Registry.Remove(key))
        {
            Debug.LogWarning($"Tutorial registry doesn't contain key {key}.");
        }
    }

    public static void StartTutorial<TTutorialAction>(TutorialDataModel<TTutorialAction> tutorial, Action tutorialCompleted) where TTutorialAction : TutorialAction, new()
    {
        LauncherActionQueue actionQueue = LauncherUI.Instance.ActionQueue;
        
        actionQueue.AddAction(CreateTutorialAction<TTutorialAction>());
        actionQueue.AddAction(new LauncherCallbackAction(tutorialCompleted, LauncherActionOrder.Tutorial));
    }

    static TTutorialAction CreateTutorialAction<TTutorialAction>() where TTutorialAction : TutorialAction, new()
    {
        TTutorialAction tutorialAction = new TTutorialAction();

        foreach (PropertyInfo property in tutorialAction.GetType().GetProperties())
        {
            TutorialPropertyAttribute tutorialProperty = property.GetCustomAttribute<TutorialPropertyAttribute>(true);
            
            if(tutorialProperty == null)
                continue;
            
            object value = Get(tutorialProperty.Key);
            
            if (value == default)
                Debug.LogWarning($"Tutorial registry doesn't have value for key {tutorialProperty.Key}");
            
            property.SetValue(tutorialAction, value);
        }

        return tutorialAction;
    }
    
    static object Get(string key)
    {
        return Registry.ContainsKey(key) ? Registry[key] : default;
    }
}
