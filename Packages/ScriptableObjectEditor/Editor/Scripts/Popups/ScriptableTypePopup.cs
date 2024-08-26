/*using UnityEditor;
using UnityEngine;

public class DropdownEditorWindow : PopupWindowContent
{
    protected Rect button;

    public DropdownEditorWindow(Object selected, Rect origin)
    {
        //selectedObject = selected;
        button = origin;
    }

    public override void OnGUI(Rect rect)
    {
        GenericMenu menu = new GenericMenu();

        var function = new GenericMenu.MenuFunction2((type) => { ScriptablesEditorWindow.activeType = (System.Type)type; ScriptablesEditorWindow.typeName = type.ToString();});

        menu.AddItem(new GUIContent("All"), OfType(typeof(ScriptableObject)), function, typeof(ScriptableObject));
        menu.AddSeparator("");

        foreach (var item in AssemblyTypes.GetAllTypes())
        {
            menu.AddItem(new GUIContent(item.ToString()), OfType(item), function, item);
        }
        menu.ShowAsContext();
    }

    protected bool OfType(System.Type type)
    {
        return ScriptablesEditorWindow.activeType == type;
    }

    public override Vector2 GetWindowSize()
    {
        return new Vector2(button.width, 125);
    }
}
*/