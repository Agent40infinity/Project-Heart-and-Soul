using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using Agent.Assembly;

namespace Agent.SOE
{
    public class CreationEditorWindow : EditorWindow
    {
        protected string scriptableName = "";

        protected string selectedPath = "Assets";
        protected System.Type selectedType = typeof(ScriptableObject);

        protected string typeName = "New Type";
        protected Rect typeButton = new Rect();

        private string createdPath = "";

        private void OnEnable()
        {
            ShowPopup();
            position = new Rect(position.x, position.y, position.width, 150);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));

            if (GUILayout.Button(new GUIContent("Folder", "Used to select the folder where the new ScriptableObject will reside."), GUILayout.MaxWidth(150)))
            {
                string basePath = EditorUtility.OpenFolderPanel("Select folder to mask path.", selectedPath, "");

                if (basePath.Contains(Application.dataPath))
                {
                    basePath = basePath.Substring(basePath.LastIndexOf("Assets"));
                }

                if (AssetDatabase.IsValidFolder(basePath))
                {
                    selectedPath = basePath;
                }
            }

            EditorGUILayout.LabelField(selectedPath);
            EditorGUILayout.EndHorizontal();

            if (EditorGUILayout.DropdownButton(new GUIContent(typeName, "The type that the new ScriptableObject will be based off."), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();

                var function = new GenericMenu.MenuFunction2((type) => { selectedType = (System.Type)type; typeName = type.ToString(); if (selectedType == typeof(ScriptableObject)) typeName = "New Type"; });

                menu.AddItem(new GUIContent("New Type"), AssemblyTypes.OfType(typeof(ScriptableObject), selectedType), function, typeof(ScriptableObject));
                menu.AddSeparator("");

                foreach (var type in AssemblyTypes.GetAllTypes())
                {
                    var typeName = AssemblyStrings.ConvertTypeToDirectory(type.ToString());

                    menu.AddItem(new GUIContent(typeName), AssemblyTypes.OfType(type, selectedType), function, type);
                }

                menu.DropDown(typeButton);
            }
            if (Event.current.type == EventType.Repaint) typeButton = GUILayoutUtility.GetLastRect();

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Name", "The name that the new ScriptableObject or Type will be called."), GUILayout.Width(40));
            scriptableName = EditorGUILayout.TextField(scriptableName);

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(30);
            EditorGUILayout.BeginHorizontal("Box");

            if (GUILayout.Button(new GUIContent("Create", "Create the new ScriptableObject with the selected folder, Type, & name.")))
            {
                createdPath = Application.dataPath.Replace("Assets", "") + selectedPath + "/" + scriptableName + ".cs";

                switch (true)
                {
                    case bool _ when scriptableName.Length <= 0:
                        EditorUtility.DisplayDialog("Error: File Name Required", "The " + selectedType + " file name can not be left empty.", "Ok");
                        break;
                    case bool _ when !scriptableName.All(char.IsLetterOrDigit):
                        EditorUtility.DisplayDialog("Error: File Name Required", "The " + selectedType + " file name can not contain invalid characters.", "Ok");
                        break;
                    case bool _ when File.Exists(createdPath):
                        EditorUtility.DisplayDialog("Error: File already exists.", "A file of that name already exists within the selected directory.", "Ok");
                        break;
                    case bool _ when selectedType == typeof(ScriptableObject):
                        var template = Resources.Load<TextAsset>("TemplateScriptable");
                        string contents = template.text;
                        contents = contents.Replace("DefaultName", scriptableName);

                        StreamWriter sw = new StreamWriter(createdPath);
                        sw.Write(contents);
                        sw.Close();
                        AssetDatabase.Refresh();
                        Created(true);
                        break;
                    default:
                        var type = selectedType;
                        Object newScriptable = AssemblyTypes.CreateObject(type);
                        AssetDatabase.CreateAsset(newScriptable, selectedPath + "/" + scriptableName + ".asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        Created(false);
                        break;
                }
            }

            if (GUILayout.Button(new GUIContent("Close", "Exit the creation menu."))) { Close(); }

            EditorGUILayout.EndHorizontal();
        }

        public void Created(bool newType)
        {
            switch (newType)
            {
                case true:
                    switch (EditorUtility.DisplayDialog(typeName + " Created! ", typeName + " '" + scriptableName + "' has been successfully created! Would you like to open and modify it's contents?", "Yes", "No"))
                    {
                        case true:
                            Application.OpenURL(createdPath);
                            Close();
                            break;
                        case false:
                            Close();
                            break;
                    }
                    break;
                case false:
                    switch (EditorUtility.DisplayDialog(typeName + " Created! ", typeName + " '" + scriptableName + "' has been successfully created!", "Ok"))
                    {
                        case true:
                            Close();
                            break;
                    }
                    break;
            }

        }
    }
}