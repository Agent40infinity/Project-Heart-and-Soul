//
// MIT License Copyright(c) 2025 Aiden Nathan, https://github.com/Agent40infinity/
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

/// <summary>
/// Lightweight Dynamic variable that contains dual types and can be toggled between.
/// 
/// PairVar was designed to be used for overloaded functions that can potentionally take in multiple Types.
/// It works well when an overload can take in a singlular type or a data collection of said type (E.g. string & List<string>)
/// 
/// Toggling the PairVar will change the type being displayed in the inspector and will act at runtime as the "Selected" value.
/// Both variables can still be referenced through their operators or as Initial & Alt.
/// Referencing the selected / toggled value will require you to use .Value which will dynamically return the ccorrect value.
/// 
/// This was specifically designed to work in tandum with SceneAttribute <see cref="https://gist.github.com/Agent40infinity/47917c5d7af72c0f7c5588026e6209f6">.
/// PairVar works well with ScriptableObject and GameObject management of Scene transitions with a custom SceneManager.
/// Best example: Level X requires 4 scenes to be loaded while Level Y requires 1 scene to be loaded, in this instance you would have a PairVar of <string, string[]>
/// </summary>
[System.Serializable]
public struct PairVar<T, L>
{
    [UnityEngine.SerializeField] private bool enabled;
    [UnityEngine.SerializeField] private T value;
    [UnityEngine.SerializeField] private L alt;

    public PairVar(T initial, L altInitial, bool isDefault)
    {
        enabled = isDefault;
        value = initial;
        alt = altInitial;
    }

    public bool Enabled => enabled;
    public T Initial => value;
    public L Alt => alt;

    /// <summary>
    /// Dynamically returns the toggled / selected value of the PairVar.
    /// </summary>
    public dynamic Value => enabled ? value : alt;

    /// <summary>
    /// Returns 'Initial' as the operator for PairVar. 
    /// Are you looking for the toggled value? Use .Value
    /// </summary>
    public static implicit operator T(PairVar<T,L> pairVar) => pairVar.enabled ? pairVar.Initial : default;

    /// <summary>
    /// Returns 'Alt' as the operator for PairVar. 
    /// Are you looking for the toggled value? Use .Value
    /// </summary>
    public static implicit operator L(PairVar<T, L> pairVar) => !pairVar.enabled ? pairVar.Alt : default;
}

#if UNITY_EDITOR
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
#endif