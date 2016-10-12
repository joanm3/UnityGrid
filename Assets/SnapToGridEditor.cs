﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorSupport;

[CustomEditor(typeof(SnapToGrid))]
public class SnapToGridEditor : Editor
{
    SnapToGrid m_myTarget;
    bool m_instantiated = false;
    private bool m_controlPressed = false;

    //GameObject m_instantiatedGameObject = new GameObject(); 

    private void OnEnable()
    {
        m_instantiated = false;
        SceneView.onSceneGUIDelegate += EventHandler;
    }

    private void OnDisable()
    {
        m_instantiated = false;
        SceneView.onSceneGUIDelegate -= EventHandler;
    }

    private void EventHandler(SceneView sceneview)
    {
        if (m_myTarget == null)
            m_myTarget = target as SnapToGrid;

        ToolsSupport.UnityHandlesHidden = m_myTarget.hideUnityHandles;

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Native));
        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 gridPos = Vector3.zero;

        RaycastHit hitInfo;
        if (Physics.Raycast(worldRay, out hitInfo, 10000))
        {
            //if (hitInfo.transform.tag == "Grid")
            //{
            gridPos = hitInfo.point;
            //}
        }
        else
        {
            gridPos = new Vector3(
                worldRay.origin.x * worldRay.direction.x,
                worldRay.origin.y * worldRay.direction.y,
                worldRay.origin.z * worldRay.direction.z);
        }

        int col = (int)gridPos.x / (int)LevelGrid.Ins.gridSize;
        int row = (int)gridPos.z / (int)LevelGrid.Ins.gridSize;

        EditorGUI.BeginChangeCheck();

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 ||
            Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            SnapToGrid(col, row);
        }

        if ((Event.current.type == EventType.keyDown) && (Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl))
            m_controlPressed = true;

        if ((Event.current.type == EventType.keyUp) && (Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl))
            m_controlPressed = false;

        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            m_instantiated = false;
            if (LevelGrid.Ins.selectedGameObject)
            {
                //make copy if control is pressed
                if (!m_controlPressed)
                {
                    Undo.IncrementCurrentGroup();
                    Undo.DestroyObjectImmediate(m_myTarget.gameObject);
                }
                Selection.activeGameObject = LevelGrid.Ins.selectedGameObject;
            }
        }
    }



    private void SnapToGrid(int col, int row)
    {
        if (m_myTarget == null)
            m_myTarget = target as SnapToGrid;

        if (!LevelGrid.Ins.snapToGrid)
            return;

        //// Check out of bounds and if we have a piece selected
        if (!LevelGrid.Ins.IsInsideGridBounds(col, row))
            return;

        GameObject obj = m_myTarget.gameObject;
        //GameObject obj;
        if (!m_instantiated)
        {
            obj = Instantiate(m_myTarget.gameObject);
            obj.name = m_myTarget.gameObject.name;
            LevelGrid.Ins.selectedGameObject = obj;
            m_instantiated = true;
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
        }
        LevelGrid.Ins.selectedGameObject.transform.position = LevelGrid.Ins.GridToWorldCoordinates(col, row);
        Undo.IncrementCurrentGroup();
    }
}
