using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pokemon/Type")]
public class Type : ScriptableObject
{
    [Header("Information")]
    public string typeName;

    [Header("Type Chart")]
    public List<Type> effective;
    public List<Type> weakness;
    public List<Type> immunity;
}
