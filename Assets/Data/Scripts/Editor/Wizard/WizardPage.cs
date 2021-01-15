using UnityEditor;
using UnityEngine;

public abstract class WizardPage
{
    protected Wizard Wizard;

    public virtual void Initialize(Wizard wizard)
    {
        Wizard = wizard;
    }
    
    public abstract string Title { get; }
    public abstract string Description { get; }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public abstract void Draw(Rect rect);

    protected virtual bool Valid => true;

    protected virtual string GetErrors()
    {
        return "No errors"; 
    }

    protected void ShowErrors()
    {
        EditorUtility.DisplayDialog("Error", GetErrors(), "Ok");
    }

    protected void Complete()
    {
        if(Valid)
            Wizard.GoNext();
        else
            ShowErrors();
    }

    protected void DrawNavigationButtons(Rect rect)
    {
        Rect buttonsRect = new Rect(rect.x, rect.y + rect.height, rect.width, 20);
        Rect prevButtonRect = new Rect(buttonsRect.x + 2, buttonsRect.y, 100, 20);
        Rect nextButtonRect = new Rect(buttonsRect.x + buttonsRect.width - 102, buttonsRect.y, 100, 20);

        Color prevColor = GUI.backgroundColor;
        
        GUI.backgroundColor = Color.red;
        if (GUI.Button(prevButtonRect, "Previous", EditorStyles.miniButtonLeft))
        {
            Wizard.GoPrevious();
        }
        
        GUI.backgroundColor = Color.green;
        if(GUI.Button(nextButtonRect, "Next", EditorStyles.miniButtonLeft))
        {
            Complete();
        }
        
        GUI.backgroundColor = prevColor;
    }

    protected bool DrawFinishButton(Rect rect)
    {
        Rect buttonsRect = new Rect(rect.x, rect.y + rect.height, rect.width, 20);
        Rect prevButtonRect = new Rect(buttonsRect.x + 2, buttonsRect.y, 100, 20);
        Rect nextButtonRect = new Rect(buttonsRect.x + buttonsRect.width - 102, buttonsRect.y, 100, 20);

        Color prevColor = GUI.backgroundColor;
        
        GUI.backgroundColor = Color.red;
        if (GUI.Button(prevButtonRect, "Previous", EditorStyles.miniButtonLeft))
            Wizard.GoPrevious();

        GUI.backgroundColor = Color.green;
        
        bool finish = GUI.Button(nextButtonRect, "FINISH", EditorStyles.miniButtonLeft);

        GUI.backgroundColor = prevColor;

        return finish;
    }
}
