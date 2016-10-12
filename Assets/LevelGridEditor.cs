using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelGrid))]
public class LevelGridEditor : Editor
{




    override public void OnInspectorGUI()
    {
        DrawDefaultInspector(); 

        if(GUILayout.Button("Open Grid Window", GUILayout.Width(255)))
        {
            OpenLevelGridWindow(); 
        }
    }

    [MenuItem("Custom/Level Grid")]
    static public void OpenLevelGridWindow()
    {
        LevelGridWindow window = (LevelGridWindow)EditorWindow.GetWindow(typeof(LevelGridWindow));
        window.Init();
    }



}
