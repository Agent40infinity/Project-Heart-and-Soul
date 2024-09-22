namespace Agent.Assembly
{
    public static class AssemblyStrings
    {
        private const int stringMax = 27;

        public static string ConvertTypeToDirectory(string typeName)
        {
            typeName = typeName.Replace(".", "/");

            switch (true)
            {
                case bool _ when typeName.Contains("Unity/"):
                    return "Unity/" + typeName;
                case bool _ when typeName.Contains("UnityEditor"):
                    return "Unity/UnityEditor/" + typeName;
                case bool _ when typeName.Contains("UnityEngine"):
                    return "Unity/UnityEngine/" + typeName;
                case bool _ when typeName.Contains("TMPro"):
                    return "Unity/TMPro/" + typeName;
                default:
                    return typeName;
            }
        }

        public static string ShortenString(string item)
        {
            switch (true)
            {
                case bool _ when item.Length >= stringMax:
                    return item.Substring(0, stringMax) + "...";
                default:
                    return item;
            }
        }
    }
}