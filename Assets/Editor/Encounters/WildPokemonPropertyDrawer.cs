using UnityEngine;
using UnityEditor;

[
    CustomPropertyDrawer(typeof(WildPokemon), true),
    CanEditMultipleObjects
]
public class WildPokemonPropertyDrawer : PropertyDrawer
{
    private float encounterHeight = EditorGUIUtility.singleLineHeight * 5;
    private float buffer = 10;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        => encounterHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var pokemonProperty = property.FindPropertyRelative("pokemon");
        var levelProperty = property.FindPropertyRelative("level");
        var rateProperty = property.FindPropertyRelative("rate");

        EditorGUI.BeginProperty(position, label, property);

        var origin = position;

        position.y += EditorGUIUtility.singleLineHeight;
        position.x += buffer;
        position.width -= buffer;
        var originWidth = position.width;

        // Left Column
        position.width = encounterHeight * 4;

        var background = new GUIStyle("Window").normal.scaledBackgrounds[0];
        var backgroundRect = new Rect(
            position.x - EditorGUIUtility.singleLineHeight / 2f,
            position.y - EditorGUIUtility.singleLineHeight / 3f,
            encounterHeight,
            encounterHeight);

        GUI.DrawTexture(backgroundRect, background);

        var pokeBase = pokemonProperty.boxedValue as PokemonBase;

        if (pokeBase != null)
        {
            EditorGUI.PrefixLabel(origin, string.IsNullOrEmpty(pokeBase.name) ? label : new GUIContent(pokeBase.name));
            GetTextureFromSprite(pokeBase.pokedexSprite, position);
        }

        // Right Column
        position.x += encounterHeight;
        position.width = originWidth - encounterHeight;
        EditorGUIUtility.labelWidth = originWidth / 8;
        EditorGUI.PropertyField(position, pokemonProperty, true);

        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, levelProperty, true);

        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, rateProperty, true);

        EditorGUI.EndProperty();
    }

    private void GetTextureFromSprite(Sprite sprite, Rect position)
    {
        if (sprite == null)
        {
            return;
        }

        var tex = sprite.texture;
        var spriteRect = sprite.textureRect;

        var aspectWidth = spriteRect.width / spriteRect.height;
        var aspectHeight = spriteRect.height / spriteRect.width;

        var uv = new Rect(
            spriteRect.x / tex.width,
            spriteRect.y / tex.height,
            spriteRect.width / tex.width,
            spriteRect.height / tex.height);

        var minPos = EditorGUIUtility.singleLineHeight / 2;
        var maxDiameter = EditorGUIUtility.singleLineHeight * 3;

        var previewRect = new Rect(
            position.x + minPos + (aspectWidth > 1 ? 0 : (maxDiameter - (maxDiameter * aspectWidth)) / 2),
            position.y + minPos + (aspectHeight > 1 ? 0 : (maxDiameter - (maxDiameter * aspectHeight)) / 2),
            maxDiameter * (aspectWidth > 1 ? 1 : aspectWidth),
            maxDiameter * (aspectHeight > 1 ? 1 : aspectHeight));

        GUI.DrawTextureWithTexCoords(previewRect, tex, uv, true);
    }
}