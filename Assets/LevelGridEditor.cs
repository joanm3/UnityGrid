using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorSupport;

[CustomEditor(typeof(LevelGrid))]
public class LevelGridEditor : Editor
{


    private void OnEnable()
    {

        SceneView.onSceneGUIDelegate += EventHandler;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= EventHandler;
    }

    private void EventHandler(SceneView sceneview)
    {

        LevelGrid _myTarget = target as LevelGrid;
        if (_myTarget)
            ToolsSupport.UnityHandlesHidden = _myTarget.hideUnityHandles; 
    }



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
