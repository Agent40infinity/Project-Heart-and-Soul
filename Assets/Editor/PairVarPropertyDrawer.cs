using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomPropertyDrawer(typeof(PairVar<,>))]
    public class PairVarPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("value");
            var altProperty = property.FindPropertyRelative("alt");
            var enabledProperty = property.FindPropertyRelative("enabled");

            switch (enabledProperty.boolValue)
            {
                case true:
                    return EditorGUI.GetPropertyHeight(valueProperty);
                case false:
                    return EditorGUI.GetPropertyHeight(altProperty);
            }
            
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("value");
            var altProperty = property.FindPropertyRelative("alt");
            var enabledProperty = property.FindPropertyRelative("enabled");

            EditorGUI.BeginProperty(position, label, property);
            position.width -= 24;
            switch (enabledProperty.boolValue)
            {
                case true:
                    EditorGUI.PropertyField(position, valueProperty, label, true);
                    break;
                case false:
                    EditorGUI.PropertyField(position, altProperty, label, true);
                    break;
            }

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            position.x += position.width + 24;
            position.width = position.height = EditorGUI.GetPropertyHeight(enabledProperty);
            position.x -= position.width;
            EditorGUI.PropertyField(position, enabledProperty, GUIContent.none);
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}