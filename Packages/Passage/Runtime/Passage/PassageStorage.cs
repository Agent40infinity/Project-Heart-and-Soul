using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PassageStorage
{
    private static string lastPassage = "";

    public static string SetTarget(string target) => lastPassage = target;
    public static string GetTarget => lastPassage;

    public static string ClearTarget() => lastPassage = "";
}
