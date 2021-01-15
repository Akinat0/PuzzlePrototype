
using UnityEditor;
using UnityEngine;

public class CollectionWizardAnimationStates : WizardPage
{
    public override string Title => "Animtions";
    public override string Description => "Here you can set animation params";

    int animationStatesAmount = 2;
    protected override bool Valid => animationStatesAmount >= 0;

    protected override string GetErrors()
    {
        if (animationStatesAmount < 0)
            return "Animations variations couldn't be less than zero";

        return base.GetErrors();
    }

    public override void Exit()
    {
        base.Exit();
        
        Wizard.SetData("collection_wizard_animations_variants", animationStatesAmount);
    }

    public override void Draw(Rect rect)
    {
        EditorGUILayout.BeginVertical();

        Rect statesAmountRect = new Rect(rect.position + new Vector2(0, rect.position.y / 2), new Vector2(200, 20));
        
        animationStatesAmount = EditorGUI.IntField(statesAmountRect,"Animation Variants", animationStatesAmount);
        
        EditorGUILayout.EndVertical();
        
        DrawNavigationButtons(rect);
    }
}
