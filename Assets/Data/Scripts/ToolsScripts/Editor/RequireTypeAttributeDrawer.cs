using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(RequireTypeAttribute))]
public class RequireTypeAttributeDrawer : PropertyDrawer
{
    RequireTypeAttribute Attribute => attribute as RequireTypeAttribute;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        DrawProperty(position, property, label);
        
        EditorGUI.EndProperty();
    }

    void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ObjectReference)
        {
            EditorGUI.HelpBox(position, "RequireTypeAttribute has wrong target", MessageType.Error);
            return;
        }

        Object targetValue = 
            EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Object), true);

        if (targetValue == property.objectReferenceValue)
            return;
        
        Object implementor;

        //if target is GameObject we'll try to find implementor among components
        if (targetValue is GameObject gameObject)
            implementor = gameObject.GetComponent(Attribute.Type);
        else
            implementor = targetValue;
        
        if (implementor == property.objectReferenceValue)
            return;

        bool implementsRequiredType = Attribute.Type.IsInstanceOfType(implementor);
        
        if (implementsRequiredType)
            property.objectReferenceValue = implementor;
    }
}