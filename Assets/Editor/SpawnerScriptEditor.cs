using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(NewSpawnerRefactored))]
public class SpawnerScriptEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        NewSpawnerRefactored spawner = (NewSpawnerRefactored)target;
    }
}
