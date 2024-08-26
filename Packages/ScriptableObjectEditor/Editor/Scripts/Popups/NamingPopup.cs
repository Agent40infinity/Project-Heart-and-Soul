using UnityEditor;
using UnityEngine;
using Agent.Assembly;

namespace Agent.SOE
{
    public class NamingPopup : PopupWindowContent
    {
        protected string newName = "Please enter a new name";
        protected Object selectedObject;
        protected Rect position;

        protected Vector2 dimensions = new Vector2(300, 125);

        public NamingPopup(Object selected, Rect origin)
        {
            selectedObject = selected;
            position = origin;
        }

        public override void OnGUI(Rect rect)
        {
            editorWindow.position = AssemblyTypes.CenterOnOriginWindow(editorWindow.position, position);

            EditorGUILayout.Space(10);
            var style = new GUIStyle(GUI.skin.label);
            style.fontSize = 12;
            style.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField("Rename " + selectedObject.name, style);
            EditorGUILayout.Space(10);

            newName = EditorGUILayout.TextField(newName);

            if (GUILayout.Button("Rename"))
            {
                var path = AssetDatabase.GetAssetPath(selectedObject);
                AssetDatabase.RenameAsset(path, newName);
                editorWindow.Close();
            }

            if (GUILayout.Button("Cancel"))
            {
                editorWindow.Close();
            }

            GUILayout.FlexibleSpace();
        }

        public override Vector2 GetWindowSize()
        {
            return dimensions;
        }
    }
}