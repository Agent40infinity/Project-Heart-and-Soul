using System.Linq;
using UnityEngine;
using UnityEditor;
using Agent.Assembly;

namespace Agent.SOE
{
    public class ScriptableObjectEditorWindow : EditorWindow
    {
        // Static reference for the window.
        public static ScriptableObjectEditorWindow window;

        // Customisation.
        protected GUISkin skin;

        // Selected Object Checks before serialization.
        protected string selectedName;
        protected Object selectedObject;
        protected ScriptableObject[] activeObjects;

        // Selected Scriptable Object for Inspector.
        [SerializeField] protected SerializedObject serializedObject;
        [SerializeField] protected SerializedProperty serializedProperty;

        // Scroll Views.
        Vector2 scrollPosition = Vector2.zero;
        Vector2 itemScrollPosition = Vector2.zero;
        readonly float sidebarWidth = 250f;

        // Selected Path.
        protected string activePath = "Assets";

        // Selected Type.
        protected System.Type activeType = typeof(ScriptableObject);
        protected string typeName = "ScriptableObjects";
        protected Rect typeButton;

        // Sidebar Menu
        protected const float inspectorWidth = 600f;
        protected bool hideMenu = false;
        protected bool showMenu = false;

        // Search Function.
        protected string sortSearch = "";

        // Pagination
        protected bool pagination = true;

        protected int pageIndex = 1;
        protected int pageSize = 100;

        // Settings
        protected enum EditorState { Scriptable, Settings, Creation };
        protected EditorState editorState = EditorState.Scriptable;

        protected bool isInspectorSelection = false;
        protected bool isRealtimeLoad = true;

        protected int PageIndexMax()
            => (int)Mathf.Ceil(AssemblyTypes.RawScriptableCount / (float)pageSize);


        [MenuItem("SOE/Scriptable Object Editor")]
        protected static void ShowWindow()
        {
            window = GetWindow<ScriptableObjectEditorWindow>("Scriptables");
            window.titleContent.image = (Texture2D)Resources.Load("Images/Icon");
            window.UpdateObjects();
        }

        private void OnEnable()
        {
            skin = (GUISkin)Resources.Load("ScriptableEditorGUI");
            titleContent.image = (Texture2D)Resources.Load("Images/Icon");
            UpdateObjects();

            isInspectorSelection = System.Convert.ToBoolean(PlayerPrefs.GetInt("InspectorSelection"));
            isRealtimeLoad = System.Convert.ToBoolean(PlayerPrefs.GetInt("realtimeLoad"));
            pageSize = PlayerPrefs.GetInt("paginationPageSize");
        }

        private void OnGUI()
        {
            WindowHandler();

            EditorGUILayout.BeginVertical("box");

            EditorHeader();
            EditorUtility();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();

            if (!hideMenu || showMenu)
            {
                ScriptableNavigationMenu();
            }

            switch (editorState)
            {
                case EditorState.Scriptable:
                    DisplaySelectedScriptable();
                    break;
                case EditorState.Settings:
                    DisplayEditorSettings();
                    break;
                case EditorState.Creation:
                    break;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void WindowHandler()
        {
            if (window == null)
            {
                window = GetWindow<ScriptableObjectEditorWindow>("SOE");
            }

            hideMenu = window.position.width < inspectorWidth;
        }

        private void EditorHeader()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField("Scriptable Object Editor", GetCustomStyle("header"));

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void EditorUtility()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

            if (hideMenu)
            {
                if (GUILayout.Button(new GUIContent("=", "Display the Scriptable Object Search Bar"), GUILayout.MaxWidth(EditorGUIUtility.singleLineHeight)))
                {
                    showMenu = !showMenu;
                }
            }

            if (GUILayout.Button(new GUIContent("Folder", "Allows you to mask the search directory."), GUILayout.MaxWidth(150)))
            {
                string basePath = UnityEditor.EditorUtility.OpenFolderPanel("Select folder to mask path.", activePath, "");
                string dataPath = Application.dataPath.Replace("/Assets", "");

                if (basePath.Contains(dataPath))
                {
                    string projectName = dataPath.Split('/').Last();
                    basePath = basePath.Substring(basePath.LastIndexOf(projectName)).Replace($"{projectName}/", "");
                }

                if (!basePath.Contains("Assets") && !basePath.Contains("Packages"))
                {
                    UnityEditor.EditorUtility.DisplayDialog("Error: File Path Invalid", "Please make sure the path is contained with the project's assets folder", "Ok");
                }
                else
                {
                    activePath = basePath;
                    UpdateObjects();
                }
            }

            EditorGUILayout.LabelField(activePath);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent((Texture2D)Resources.Load("Images/Settings"), "Display the settings menu for the Scriptable Object Editor."), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight)))
            {
                if (editorState == EditorState.Settings)
                {
                    editorState = EditorState.Scriptable;
                }
                else
                {
                    editorState = EditorState.Settings;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void ScriptableNavigationMenu()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(sidebarWidth), GUILayout.ExpandHeight(true));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Type:", GUILayout.MaxWidth(30));
            if (EditorGUILayout.DropdownButton(new GUIContent(typeName, "Used to mask the type of ScriptableObject being searched for."), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();

                var function = new GenericMenu.MenuFunction2((type) => { activeType = (System.Type)type; typeName = type.ToString(); if (activeType == typeof(ScriptableObject)) typeName = "All"; pageIndex = 1; UpdateObjects(); });

                menu.AddItem(new GUIContent("All", "Display every type of ScriptableObject within the project."), AssemblyTypes.OfType(typeof(ScriptableObject), activeType), function, typeof(ScriptableObject));
                menu.AddSeparator("");

                foreach (var type in AssemblyTypes.GetAllTypes())
                {
                    var typeName = AssemblyStrings.ConvertTypeToDirectory(type.ToString());

                    menu.AddItem(new GUIContent(typeName), AssemblyTypes.OfType(type, activeType), function, type);
                }
                menu.DropDown(typeButton);
            }
            if (Event.current.type == EventType.Repaint) typeButton = GUILayoutUtility.GetLastRect();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            sortSearch = EditorGUILayout.TextField(sortSearch, GUILayout.MaxWidth(240));

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.ExpandHeight(true));

            if (activeObjects.Length > 0)
            {
                DrawScriptableNavigation(activeObjects);
            }

            EditorGUILayout.EndScrollView();

            if (pagination)
            {
                PaginationController();
            }

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(new GUIContent("+", "Open the ScriptableObject creation menu."), GUILayout.Width(25)))
            {
                CreationEditorWindow window = GetWindow<CreationEditorWindow>();
                window.Init(activePath, activeType, AssemblyTypes.CenterOnOriginWindow(window.position, position));
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        protected void PaginationController()
        {
            var needsUpdating = false;

            EditorGUILayout.BeginHorizontal("box");

            if (GUILayout.Button(new GUIContent("<<", "Page down by a multiple, more info in settings."), GUILayout.Width(25)) && pageIndex > 1)
            {
                pageIndex = pageIndex - 10 > 1 ? pageIndex -= 10 : pageIndex = 0;

                needsUpdating = true;
            }

            if (GUILayout.Button(new GUIContent("<", "Page down by 1."), GUILayout.Width(25)) && pageIndex > 1)
            {
                pageIndex--;
                needsUpdating = true;
            }

            GUILayout.FlexibleSpace();
            GUILayout.TextField(string.Format("{0} / {1}", pageIndex, PageIndexMax()));
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent(">", "Page up by 1."), GUILayout.Width(25)) && pageIndex < PageIndexMax())
            {
                pageIndex++;
                needsUpdating = true;
            }

            if (GUILayout.Button(new GUIContent(">>", "Page up by a multiple, more info in settings."), GUILayout.Width(25)) && pageIndex < PageIndexMax())
            {
                pageIndex = pageIndex + 10 < PageIndexMax() ? pageIndex += 10 : pageIndex = PageIndexMax();

                needsUpdating = true;
            }

            EditorGUILayout.EndHorizontal();

            if (needsUpdating)
            {
                UpdateObjects();
            }
        }

        protected void DrawScriptableNavigation(ScriptableObject[] objects)
        {
            foreach (ScriptableObject scriptable in objects)
            {
                if (scriptable != null && scriptable.name.IndexOf(sortSearch, System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (GUILayout.Button(AssemblyStrings.ShortenString(scriptable.name), scriptable == selectedObject ? GetCustomStyle("selectedButton") : skin.button, GUILayout.ExpandWidth(true)))
                    {
                        if (Event.current.button == 1)
                        {
                            GenericMenu menu = new GenericMenu();

                            var rename = new GenericMenu.MenuFunction2((name) => RenamePopup(AssemblyTypes.FindObject(activeObjects, scriptable.name)));
                            var delete = new GenericMenu.MenuFunction2((name) => DeleteScriptableObject(scriptable, false));

                            menu.AddItem(new GUIContent("Rename"), false, rename, scriptable.name);
                            menu.AddItem(new GUIContent("Delete"), false, delete, scriptable);
                            menu.ShowAsContext();
                        }
                        else
                        {
                            selectedObject = scriptable;
                            serializedObject = new SerializedObject(selectedObject);

                            if (isInspectorSelection)
                            {
                                Selection.activeObject = scriptable;
                            }

                            editorState = EditorState.Scriptable;
                        }
                    }
                }
            }
        }

        private void DisplaySelectedScriptable()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

            switch (true)
            {
                case bool _ when serializedObject != null && selectedObject != null:
                    itemScrollPosition = EditorGUILayout.BeginScrollView(itemScrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.ExpandHeight(true));

                    bool isDirty = !isRealtimeLoad && serializedObject != null && UnityEditor.EditorUtility.IsDirty(serializedObject.targetObject);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("<color=silver>Name:</color> " + serializedObject.FindProperty("m_Name").stringValue + (isDirty ? "*" : ""), GetCustomStyle("scriptableName"));
                    GUILayout.FlexibleSpace();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField(isDirty ? "Unsaved" : "");

                    if (GUILayout.Button(new GUIContent("Rename", "Use to rename the selected ScriptableObject."), GUILayout.Width(100)))
                    {
                        RenamePopup(selectedObject);
                    }

                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(EditorGUIUtility.singleLineHeight);

                    EditorGUI.BeginChangeCheck();

                    serializedObject.UpdateIfRequiredOrScript();
                    serializedProperty = serializedObject.GetIterator();
                    serializedProperty.NextVisible(true);
                    DrawScriptableProperties(serializedProperty);

                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                    }

                    GUILayout.Space(EditorGUIUtility.singleLineHeight);

                    EditorGUILayout.EndScrollView();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(new GUIContent("Delete", "Use to delete the selected ScriptableObject."), GetCustomStyle("deleteButton"), GUILayout.Width(75)))
                    {
                        DeleteScriptableObject(selectedObject, true);
                    }

                    EditorGUILayout.EndHorizontal();
                    break;
                default:
                    EditorGUILayout.LabelField("No item selected, make sure you've selected an item.");
                    break;
            }

            EditorGUILayout.EndVertical();
        }

        protected void DrawScriptableProperties(SerializedProperty property)
        {
            if (property.displayName == "Script") { GUI.enabled = false; }
            EditorGUILayout.PropertyField(property, true);
            GUI.enabled = true;

            EditorGUILayout.Space(40);

            while (property.NextVisible(false))
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        protected void DisplayEditorSettings()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

            EditorGUILayout.LabelField("Settings", GetCustomStyle("scriptableName"));

            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField("<color=silver>Scriptables</color>", GetCustomStyle("settingsHeader"));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(15);
            isInspectorSelection = EditorGUILayout.Toggle(new GUIContent("Select in Inspector:", "Anytime a Scriptable is selected in SOE, it'll be highlighted in the project and inspector."), isInspectorSelection);
            PlayerPrefs.SetInt("InspectorSelection", System.Convert.ToInt32(isInspectorSelection));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(15);
            isRealtimeLoad = EditorGUILayout.Toggle(new GUIContent("Saves in Real-time:", "Regardless of ticked, changes to objects are set dirty by default."), isRealtimeLoad);
            PlayerPrefs.SetInt("realtimeLoad", System.Convert.ToInt32(isRealtimeLoad));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

            EditorGUILayout.LabelField("<color=silver>Pagination</color>", GetCustomStyle("settingsHeader"));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(15);
            pageSize = EditorGUILayout.IntField(new GUIContent("Page Size:", "How many scriptables will be displayed per page on the navigation sidebar."), pageSize);
            pageSize = pageSize == 0 ? 1 : pageSize;
            PlayerPrefs.SetInt("paginationPageSize", pageSize);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

            EditorGUILayout.EndVertical();
        }

        protected void DeleteScriptableObject(Object selected, bool clearSelected)
        {
            if (UnityEditor.EditorUtility.DisplayDialog("Delete " + selected.name + "?", "Are you sure you want to delete '" + selected.name + "'?", "Yes", "No"))
            {
                string path = AssetDatabase.GetAssetPath(selected);
                AssetDatabase.DeleteAsset(path);

                if (clearSelected)
                {
                    serializedObject = null;
                    selectedObject = null;
                    UpdateObjects();
                }
            }
        }

        protected void RenamePopup(Object selected)
        {
            PopupWindow.Show(new Rect(), new NamingPopup(selected, position));
        }

        protected void UpdateObjects()
        {
            activeObjects = AssemblyTypes.GetAllInstancesOfType(activePath, activeType, pageIndex, pageSize);
        }

        protected GUIStyle GetCustomStyle(string customStyle)
            => skin.customStyles.ToList().Find(x => x.name == customStyle);
    }
}