using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(SnapToGrid))]
public class SnapToGridEditor : Editor
{
    SnapToGrid m_myTarget;
    bool m_instantiated = false;
    private bool m_controlPressed = false;
    float distance;
    Vector3 gridPos = new Vector3();
    GameObject onMouseOverGameObject;
    bool isThisObject = false;

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

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Native));
        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        RaycastHit[] hits =  Physics.RaycastAll(worldRay, 1000);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Grid"))
            {
                gridPos = hits[i].point;
            }
            else
            {
                //only snaptogrid gameobjects are selectables
                if (hits[i].transform.GetComponent<SnapToGrid>() != null)
                {
                    onMouseOverGameObject = hits[i].transform.gameObject;
                }
                else
                {
                    onMouseOverGameObject = null;
                }
            }

        }

        //mouse position in the grid
        float col = (float)gridPos.x / ((float)LevelGrid.Ins.gridSize * LevelGrid.Ins.scaleFactor);
        float row = (float)gridPos.z / ((float)LevelGrid.Ins.gridSize * LevelGrid.Ins.scaleFactor);

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if (onMouseOverGameObject == m_myTarget.gameObject)
            {
                isThisObject = true;
            }
            else
            {
                isThisObject = false;
                if (onMouseOverGameObject != null)
                    Selection.activeGameObject = onMouseOverGameObject;
            }
        }

        if (!isThisObject)
            return;

        //mouse click and dragandrop
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            SnapToGrid((int)col, (int)row, LevelGrid.Ins.height);
        }

        LevelGrid.Ins.UpdateInputGridHeight();

        //check if control is pressed. 
        if ((Event.current.type == EventType.keyDown) && (Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl))
            m_controlPressed = true;

        if ((Event.current.type == EventType.keyUp) && (Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl))
            m_controlPressed = false;


        //if mouse released when control pressed, make a copy / otherwise, destroy old object. 
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



    private void SnapToGrid(int col, int row, float height)
    {
        if (m_myTarget == null)
            m_myTarget = target as SnapToGrid;

        if (!LevelGrid.Ins.snapToGrid)
            return;

        // Check out of bounds and if we have a piece selected
        if (!LevelGrid.Ins.IsInsideGridBounds(col, row))
            return;

        GameObject obj = m_myTarget.gameObject;
        if (!m_instantiated)
        {

            if (PrefabUtility.GetPrefabParent(Selection.activeObject) != null)
            {
                obj = PrefabUtility.InstantiatePrefab(PrefabUtility.GetPrefabParent(Selection.activeObject) as GameObject) as GameObject;
            }
            else
            {
                //Debug.Log("prefab parent not found");
                obj = Instantiate(m_myTarget.gameObject);
            }

            obj.name = m_myTarget.gameObject.name;

            LevelGrid.Ins.selectedGameObject = obj;
            m_instantiated = true;
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
        }
        LevelGrid.Ins.selectedGameObject.transform.position = LevelGrid.Ins.GridToWorldCoordinates(col, row, height);
        Undo.IncrementCurrentGroup();
    }
}
