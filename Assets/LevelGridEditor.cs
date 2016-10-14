using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorSupport;

[CustomEditor(typeof(LevelGrid))]
public class LevelGridEditor : Editor
{
    LevelGrid _myTarget;

    private void OnEnable()
    {
        _myTarget = target as LevelGrid;
        _myTarget.boxCollider = _myTarget.GetComponent<BoxCollider>();
        SceneView.onSceneGUIDelegate += EventHandler;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= EventHandler;
    }

    private void EventHandler(SceneView sceneview)
    {
        if (!_myTarget)
            _myTarget = target as LevelGrid;
        if (_myTarget)
            ToolsSupport.UnityHandlesHidden = _myTarget.hideUnityHandles;

        _myTarget.transform.position = Vector3.zero;

        float cols = _myTarget.sizeColums;
        float rows = _myTarget.sizeRows;

        _myTarget.boxCollider.size = new Vector3(cols, 0f, rows);
        _myTarget.boxCollider.center = new Vector3(cols / 2f, (float)_myTarget.height, rows / 2f);

        LevelGrid.Ins.UpdateGridHeight(); 
    }



    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Open Grid Window", GUILayout.Width(255)))
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
