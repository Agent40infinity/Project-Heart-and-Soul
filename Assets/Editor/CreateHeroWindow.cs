/*using UnityEngine;
using UnityEditor;



public class CreateHeroWindow : EditorWindow
{
    private SerializedObject serializedObject;
    private SerializedProperty serializedProperty;

    protected ScriptableObj[] heros;
    public ScriptableObj newHero;

    private void OnGUI()
    {

        serializedObject = new SerializedObject(newHero);
        serializedProperty = serializedObject.GetIterator();
        serializedProperty.NextVisible(true);
        DrawProperties(serializedProperty);
        if (GUILayout.Button("save"))
        {
            heros = GetAllInstances<ScriptableObj>();
            if (newHero.HerosName == null)
            {
                newHero.HerosName = "hero" + (heros.Length + 1);
            }
            AssetDatabase.CreateAsset(newHero, "Assets/Scripts/hero" + (heros.Length + 1) + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Close();
        }

        Apply();
    }

    protected void DrawProperties(SerializedProperty p)
    {

        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);

        }
    }

    public static T[] GetAllInstances<T>() where T : ScriptableObj
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;

    }

    protected void Apply()
    {
        serializedObject.ApplyModifiedProperties();
    }
}*/