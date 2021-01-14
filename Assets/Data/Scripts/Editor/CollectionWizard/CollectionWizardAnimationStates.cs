
using UnityEditor;
using UnityEngine;

public class CollectionWizardAnimationStates : WizardPage
{
    public override string Title => "Animtions";
    public override string Description => "Here you can set animation params";

    int animationStatesAmount = 2;

    public override void Exit()
    {
        base.Exit();
        
        Wizard.SetData("collection_wizard_animations_variants", animationStatesAmount);
    }

    public override void Draw(Rect rect)
    {
        EditorGUILayout.BeginVertical();

        animationStatesAmount = EditorGUI.IntField(rect,"Animation Variants", animationStatesAmount);
        
        EditorGUILayout.EndVertical();
        
        DrawNavigationButtons(rect);
    }
}
