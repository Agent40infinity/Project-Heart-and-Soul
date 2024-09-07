using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Passage))]
    public class PassagePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var handleProperty = property.FindPropertyRelative("handle");
            return EditorGUI.GetPropertyHeight(handleProperty);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            var handleProperty = property.FindPropertyRelative("handle");
            var passageProperty = property.FindPropertyRelative("connection");

            EditorGUI.BeginProperty(position, label, property);

            Rect col = position;

            col.width = position.width * 0.7f;
            Rect handlePos = col;
            EditorGUI.PropertyField(handlePos, handleProperty, label);

            if (handleProperty.objectReferenceValue != null)
            {
                SerializedObject handle = new SerializedObject(property.FindPropertyRelative("handle").objectReferenceValue);
                var connections = handle.FindProperty("connections");

                string[] available = new string[connections.arraySize];

                for (var i = 0; i < connections.arraySize; i++)
                {
                    available[i] = connections.GetArrayElementAtIndex(i).FindPropertyRelative("identity").stringValue;
                }

                col.x += col.width + 5;
                col.width = position.width * 0.3f - 5;

                passageProperty.intValue = EditorGUI.Popup(col, "", passageProperty.intValue, available);
            }

            EditorGUI.EndProperty();
        }
    }
}