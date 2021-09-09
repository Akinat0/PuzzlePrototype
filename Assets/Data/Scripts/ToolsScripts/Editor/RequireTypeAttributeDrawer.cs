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

        if (targetValue == null)
        {
            property.objectReferenceValue = null;
            return;
        }

        Object implementor;

        switch (targetValue)
        {
            case GameObject gameObject:
                implementor = gameObject.GetComponent(Attribute.Type);
                break;
            case Component component:
                implementor = component.gameObject.GetComponent(Attribute.Type);
                break;
            default:
                implementor = targetValue;
                break;
        }
        
        if (implementor == property.objectReferenceValue)
            return;

        bool implementsRequiredType = Attribute.Type.IsInstanceOfType(implementor);
        
        if (implementsRequiredType)
            property.objectReferenceValue = implementor;
    }
}