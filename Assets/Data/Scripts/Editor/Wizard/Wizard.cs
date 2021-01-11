using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Wizard : EditorWindow
{
    public static void Show<T>() where T : Wizard 
    {
        T window = GetWindow<T>(true, string.Empty);
        window.minSize                    = new Vector2(700, 450);
        window.maxSize                    = new Vector2(700, 450);
        window.wantsMouseEnterLeaveWindow = true;
        window.wantsMouseMove             = true;
        window.Show();
        window.Initialize();
    }
    
    #region Styles
    
    static GUIStyle titleStyle;

    public static GUIStyle TitleStyle
    {
        get
        {
            if (titleStyle == null)
            {
                titleStyle = new GUIStyle(EditorStyles.whiteBoldLabel);
                titleStyle.fontSize = 20;
                titleStyle.alignment = TextAnchor.MiddleCenter;
                titleStyle.normal.textColor = Color.black;
            }

            return titleStyle;
        }
    }

    static GUIStyle blackText;
    
    public static GUIStyle BlackLabel
    {
        get
        {
            if (blackText == null)
            { 
                blackText = new GUIStyle(EditorStyles.label);
                blackText.normal.textColor = Color.black;
            }

            return blackText;
        }
    }
    
    #endregion
    
    protected abstract WizardPage[] Pages { get; }
    
    int pageNumber;
    protected int PageNumber { get; private set; }

    protected WizardPage CurrentPage => Pages[PageNumber];

    protected readonly Dictionary<string, object> SavedData = new Dictionary<string, object>();
    
    public void GoNext()
    {
        CurrentPage.Exit();
        PageNumber++;
        CurrentPage.Enter();
    }

    public void GoPrevious()
    {
        CurrentPage.Exit();
        PageNumber--;
        CurrentPage.Enter();
    }
    
    public void SetData(string key, object value)
    {
        SavedData[key] = value;
    }

    public T GetData<T>(string key)
    {
        if (SavedData[key] is T castedData)
            return castedData;
        
        Debug.LogWarning($"Data at {key} doesn't match with type {typeof(T).Name}");
        
        return default;
    }
    
    protected virtual void Initialize()
    {
        foreach (WizardPage page in Pages)
            page.Initialize(this);
        
        Pages[PageNumber].Enter();
    }
    
    void OnGUI()
    {
        Rect titleRect = new Rect(0, 0, position.width, 40);
        Rect pageRect   = new Rect(0, 40, position.width, position.height - 80);

        DrawBackground();

        DrawHeader(titleRect);
        
        DrawPage(pageRect);
    }
    
    void DrawBackground()
    {
        EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), Color.grey);
    }

    void DrawHeader(Rect headerRect)
    {
        EditorGUI.LabelField(headerRect, CurrentPage.Title, TitleStyle);
    }
    
    void DrawPage(Rect pageRect)
    {
        Pages[PageNumber].Draw(pageRect);
    }

}