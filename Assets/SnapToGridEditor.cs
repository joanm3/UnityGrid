using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SnapToGrid))]
public class SnapToGridEditor : Editor
{
    SnapToGrid m_myTarget;
    bool m_instantiated = false;
    //GameObject m_instantiatedGameObject = new GameObject(); 

    private void OnEnable()
    {
        m_instantiated = false; 
        SnapToGrid _myTarget = target as SnapToGrid;
    }

    private void OnSceneGUI()
    {
        EventHandler();
    }

    private void EventHandler()
    {
        if (m_myTarget == null)
            m_myTarget = target as SnapToGrid;

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

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 ||
            Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {

            EditorGUI.BeginChangeCheck();
            SnapToGrid(col, row);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Position");
                //DestroyImmediate(m_instantiatedGameObject);
                //m_instantiatedGameObject = new GameObject();
                //m_instantiated = false; 
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

        //GameObject obj = _myTarget.gameObject;
        GameObject obj = new GameObject();
        if (!m_instantiated)
        {
            obj = Instantiate(m_myTarget.gameObject);
            m_instantiated = true; 
        }
        //Debug.Log(obj.name); 
        //m_instantiatedGameObject = obj; 
        obj.transform.position = LevelGrid.Ins.GridToWorldCoordinates(col, row);

        
    }
}
