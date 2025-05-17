#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

[System.Serializable]
public class WildPokemon
{
    public PokemonBase pokemon;
    [UnityEngine.SerializeField] public WildLevel level;
    public int rate;
}

[System.Serializable]
public class WildLevel
{
    public int minLvl;
    public int maxLvl;
}

#if UNITY_EDITOR
[
    CustomPropertyDrawer(typeof(WildLevel), true),
    CanEditMultipleObjects
]
public class WildLevelPropertyDrawer : PropertyDrawer
{
    private float buffer = 10;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        => EditorGUIUtility.singleLineHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var minLevelProperty = property.FindPropertyRelative("minLvl");
        var maxLevelProperty = property.FindPropertyRelative("maxLvl");

        EditorGUI.BeginProperty(position, label, property);

        position.width = position.width / 2 - (buffer / 2);

        EditorGUI.PropertyField(position, minLevelProperty, true);

        position.x += position.width + buffer;
        EditorGUI.PropertyField(position, maxLevelProperty, true);

        EditorGUI.EndProperty();
    }
}
#endif