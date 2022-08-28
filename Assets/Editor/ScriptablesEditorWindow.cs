using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

public class SriptablesEditorWindow : EditorWindow
{

    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;

    protected ScriptableObject[] activeObjects;
    protected string selectedPropertyPach;
    protected string selectedProperty;

    Vector2 scrollPosition = Vector2.zero;
    Vector2 itemScrollPosition = Vector2.zero;
    readonly float sidebarWidth = 250f;

    protected string scriptableName = "";

    [MenuItem("Tools/Scriptable Object Editor")]
    protected static void ShowWindow()
    {
        GetWindow<SriptablesEditorWindow>("Scriptables Editor");
    }

    private void OnGUI()
    {
        activeObjects = GetAllInstances<ScriptableObject>();
        serializedObject = new SerializedObject(activeObjects[0]);
        HeaderNavigation();

        EditorGUILayout.BeginHorizontal();

        SelectionNavigation();
        SelectableContents();

        EditorGUILayout.EndHorizontal();

        Apply();
    }

    private void HeaderNavigation()
    {
        EditorGUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("Editor");
        EditorGUILayout.EndHorizontal();
    }

    public void Activate()
    { 
        
    }

    private void SelectionNavigation()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(sidebarWidth), GUILayout.ExpandHeight(true));

        if (EditorGUILayout.DropdownButton(new GUIContent("Scriptable Types"), FocusType.Keyboard))
        {

        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.ExpandHeight(true));
        DrawScriptables(activeObjects);
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        scriptableName = EditorGUILayout.TextField(scriptableName);
        if (GUILayout.Button("+", GUILayout.Width(30)))
        {
            if (scriptableName.Length <= 0)
            {
                EditorUtility.DisplayDialog("Error: File Name Required", "The " + activeObjects[0].GetType() + " file name can not be left empty.", "Ok");
            }
            else if (!scriptableName.All(char.IsLetterOrDigit))
            {
                EditorUtility.DisplayDialog("Error: File Name Required", "The " + activeObjects[0].GetType() + " file name can not contain invalid characters.", "Ok");
            }
            else
            {
                var type = activeObjects[0].GetType();
                Object newScriptable = CreateObject(type);
                AssetDatabase.CreateAsset(newScriptable, "Assets/" + scriptableName + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        EditorGUILayout.EndHorizontal();
            
        EditorGUILayout.EndVertical();
    }

    private void SelectableContents()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        itemScrollPosition = EditorGUILayout.BeginScrollView(itemScrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.ExpandHeight(true));

        switch (true)
        {
            case bool x when selectedProperty != null:
                for (int i = 0; i < activeObjects.Length; i++)
                {
                    if (activeObjects[i].name == selectedProperty)
                    {
                        serializedObject = new SerializedObject(activeObjects[i]);
                        serializedProperty = serializedObject.GetIterator();
                        serializedProperty.NextVisible(true);
                        DrawProperties(serializedProperty);
                    }
                }
                break;
            default:
                EditorGUILayout.LabelField("No item selected, make sure you've selected an item.");
                break;

        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    public static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { "Assets" });
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;
    }

    public static Object CreateObject(System.Type type)
    {
        return CreateInstance(type);
    }

    protected void DrawProperties(SerializedProperty property)
    {
        while (property.NextVisible(false))
        {
            EditorGUILayout.PropertyField(property, true);
        }
    }

    protected void DrawScriptables(ScriptableObject[] objects)
    {
        foreach (ScriptableObject item in objects)
        {
            if (GUILayout.Button(item.name))
            {
                selectedPropertyPach = item.name;
            }
        }

        if (!string.IsNullOrEmpty(selectedPropertyPach))
        {
            selectedProperty = selectedPropertyPach;
        }
    }

    protected void Apply()
    {
        serializedObject.ApplyModifiedProperties();
    }
}