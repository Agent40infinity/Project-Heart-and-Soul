using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Encounters), true)]
public class EncountersPropertyDrawer : UnityEditor.Editor
{
    protected SerializedObject encounterTable;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var encounters = target as Encounters;

        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * 2);

        if (encounters.encounterTable != null)
        {
            if (encounterTable == null)
            {
                encounterTable = new SerializedObject(encounters.encounterTable);
            }

            EditorGUI.BeginChangeCheck();

            encounterTable.UpdateIfRequiredOrScript();
            var serializedProperty = encounterTable.GetIterator();
            serializedProperty.NextVisible(true);
            DrawScriptableProperties(serializedProperty);

            if (EditorGUI.EndChangeCheck())
            {
                encounterTable.ApplyModifiedProperties();
            }
        }
        else
        {
            EditorGUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("Add a EncounterTable to view.");
            EditorGUILayout.EndHorizontal();
        }
    }

    protected void DrawScriptableProperties(SerializedProperty property)
    {
        while (property.NextVisible(false))
        {
            EditorGUILayout.PropertyField(property, true);
        }
    }
}