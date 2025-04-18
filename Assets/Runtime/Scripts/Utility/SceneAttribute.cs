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
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
#endif

/// <summary>
/// Lightweight Attribute for string that allows Scene referencing in the Inspector.
/// Only used on strings and data collections that contain strings (Arrays, Lists, Collections)
/// 
/// Uses a property drawer to replace the default string drawer and replaces it with an object field for SceneAssets.
/// Additionally, when a SceneAttribute is not null, you can add & remove the scene from the build settings with a button.
/// 
/// Works by extracting the scenePath from the selected SceneAsset and can be used during runtime (SceneManager).
/// 
/// If the SceneAsset is moved, it'll keep track and replace the string automatically.
/// If the SceneAsset is renamed, it'll lose reference as the path and name of the asset are no longer related.
/// </summary>
public class SceneAttribute : UnityEngine.PropertyAttribute { }

#if UNITY_EDITOR
[
	CustomPropertyDrawer(typeof(SceneAttribute), true),
	CanEditMultipleObjects
]
internal class SceneAttributeDrawer : PropertyDrawer
{
	const float buildPadding = 5f;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (property.propertyType != SerializedPropertyType.String)
		{
			Debug.LogError(
				"<color='orange'>[Scene]</color> - Attribute only works on Type 'String' and not '" +
				property.propertyType.ToString() +
				"'.\nIs it with the correct variable?\n<a>" +
				property.serializedObject.targetObject +
				"/" + fieldInfo.ToString() + "</a>");

			EditorGUI.PropertyField(position, property);
			return;
		}

		SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

		if (!scene && !string.IsNullOrEmpty(property.stringValue))
		{
			var guids = AssetDatabase.FindAssets("t:" + typeof(SceneAsset).Name);

			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);

				if (Path.GetFileNameWithoutExtension(path) == Path.GetFileNameWithoutExtension(property.stringValue))
				{
					property.stringValue = path;
					scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);
				}
			}
		}

		EditorGUI.BeginProperty(position, label, property);

		if (!string.IsNullOrEmpty(property.stringValue))
		{
			position.width -= EditorGUIUtility.singleLineHeight + buildPadding;
		}

		scene = (SceneAsset)EditorGUI.ObjectField(position, label, scene, typeof(SceneAsset), true);
		property.stringValue = AssetDatabase.GetAssetPath(scene);

		if (!string.IsNullOrEmpty(property.stringValue))
		{
			var guid = AssetDatabase.GUIDFromAssetPath(property.stringValue);
			var inBuild = EditorBuildSettings.scenes.Any(s => s.guid == guid);

			GUIContent buildContent = inBuild ?
				new GUIContent("-", "Scene currently in build, Remove from Build?")
				:
				new GUIContent("+", "Scene currently not in build, Add it?");

			GUI.backgroundColor = inBuild ? Color.red : Color.green;

			position.x += position.width + buildPadding;
			position.width = EditorGUIUtility.singleLineHeight;

			if (GUI.Button(position, buildContent, EditorStyles.miniButtonRight))
			{
				var scenes = EditorBuildSettings.scenes.ToList();

				if (inBuild)
				{
					scenes.RemoveAll(s => s.guid == guid);
				}
				else
				{
					scenes.Add(new EditorBuildSettingsScene(guid, true));
				}

				EditorBuildSettings.scenes = scenes.ToArray();
			}
		}

		GUI.backgroundColor = default;
		EditorGUI.EndProperty();
	}
}
#endif