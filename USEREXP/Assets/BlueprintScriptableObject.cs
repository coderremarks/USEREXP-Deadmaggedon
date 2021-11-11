using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blueprint Template", menuName = "Scriptable Objects/Blueprint")]
public class BlueprintScriptableObject : ScriptableObject
{
    public Structure structure;
    public int cost;
    
}
