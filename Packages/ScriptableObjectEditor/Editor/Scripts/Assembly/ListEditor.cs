using System.Collections.Generic;
using UnityEditor;

namespace Agent.Assembly
{
    public static class ListEditor
    {
        private static List<string> forbiddenTypes = new List<string>() { "char" };

        public static bool ForbiddenType(string typeAsString)
            => forbiddenTypes.Contains(typeAsString);

        private static Dictionary<object, bool> displayed = new Dictionary<object, bool>();

        public static void ClearDisplayed()
            => displayed.Clear();

        public static void Format(SerializedProperty property)
        {
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            var list = property.serializedObject.targetObject;

            if (!displayed.ContainsKey(list))
            {
                displayed.Add(list, true);
            }

            displayed[list] = EditorGUILayout.Foldout(displayed[list], property.displayName, true);

            if (displayed[list])
            {
                for (int i = 0; i < property.arraySize; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    var field = property.GetArrayElementAtIndex(i);

                    EditorGUILayout.PropertyField(field, true);

                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
        }
    }
}