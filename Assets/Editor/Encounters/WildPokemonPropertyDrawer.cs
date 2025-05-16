using System.Collections;
using System.Collections.Generic;
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
        var minLevelProperty = property.FindPropertyRelative("minLvl");
        var maxLevelProperty = property.FindPropertyRelative("maxLvl");
        var rateProperty = property.FindPropertyRelative("rate");

        EditorGUI.BeginProperty(position, label, property);

        position.y += EditorGUIUtility.singleLineHeight;
        position.x += buffer;
        position.width -= buffer;
        var originWidth = position.width;

        // Left Column
        position.width = encounterHeight;
        var pokeBase = pokemonProperty.boxedValue as PokemonBase;

        if (pokeBase != null)
        {
            var sprite = GetTextureFromSprite(pokeBase.pokedexSprite);

            EditorGUI.LabelField(position, new GUIContent(sprite));
        }
        else
        {
            EditorGUI.LabelField(position, new GUIContent("Placeholder"));
        }

        // Right Column
        position.x += encounterHeight;
        position.width = originWidth - encounterHeight;

        EditorGUI.PropertyField(position, pokemonProperty, true);

        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, minLevelProperty, true);

        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, maxLevelProperty, true);

        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, rateProperty, true);

        EditorGUI.EndProperty();
    }

    private Texture GetTextureFromSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            return null;
        }

        var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);

        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        return croppedTexture;
    }
}