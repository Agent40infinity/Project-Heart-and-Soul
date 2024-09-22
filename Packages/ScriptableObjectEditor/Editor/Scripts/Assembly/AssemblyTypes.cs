using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Agent.Assembly
{
    public static class AssemblyTypes
    {
        public static int RawScriptableCount;

        public static System.Type[] ReturnTypes()
        {
            List<System.Type> types = new List<System.Type>();
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic);
            foreach (var item in assemblies)
            {
                types = types.Concat(item.GetTypes()).ToList();
            }

            return types.ToArray();
        }

        public static ScriptableObject[] GetAllInstancesOfType(string activePath, System.Type activeType)
        {
            string[] guids = AssetDatabase.FindAssets("t:" + activeType.Name, new[] { activePath });
            ScriptableObject[] so = new ScriptableObject[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                so[i] = (ScriptableObject)AssetDatabase.LoadAssetAtPath(path, activeType);
            }
            return so;
        }

        public static ScriptableObject[] GetAllInstancesOfType(string activePath, System.Type activeType, int pageIndex, int pageSize)
        {
            string[] rawGuids = AssetDatabase.FindAssets("t:" + activeType.Name, new[] { activePath });
            RawScriptableCount = rawGuids.Length;

            var guids = rawGuids.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

            ScriptableObject[] so = new ScriptableObject[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                so[i] = (ScriptableObject)AssetDatabase.LoadAssetAtPath(path, activeType);
            }
            return so;
        }

        public static System.Type[] GetAllTypes()
        {
            System.Type[] types = ReturnTypes();
            System.Type[] possible = (from System.Type type in types where type.IsSubclassOf(typeof(ScriptableObject)) select type).ToArray();

            return possible;
        }

        public static ScriptableObject FindObject(ScriptableObject[] objects, string property)
        {
            return objects.First(x => x.name == property);
        }

        public static Object CreateObject(System.Type type)
        {
            return ScriptableObject.CreateInstance(type);
        }

        public static Rect CenterOnOriginWindow(Rect window, Rect origin)
        {
            var pos = window;
            float w = (origin.width - pos.width) * 0.5f;
            float h = (origin.height - pos.height) * 0.5f;
            pos.x = origin.x + w;
            pos.y = origin.y + h;
            return pos;
        }

        public static bool OfType(System.Type type, System.Type compareType)
        {
            return compareType == type;
        }
    }
}