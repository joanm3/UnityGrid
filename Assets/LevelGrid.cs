using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class LevelGrid : MonoBehaviour
{
    //public GameObject testToGrid; 
    public static LevelGrid Ins;

    public enum Pow2 { g0 = 0, g1 = 1, g2 = 2, g4 = 4, g8 = 8, g16 = 16, g32 = 32, g64 = 64, g128 = 128, g256 = 256, g512 = 512, g1024 = 1024, g2048 = 2048 }
    public bool snapToGrid = true;
    public Pow2 gridSize = Pow2.g128;
    public float height;
    [Tooltip("Standard 3DSMax = 0.0254")]
    public float scaleFactor = 0.0254f;
    [HideInInspector]
    public GameObject selectedGameObject;

    [SerializeField]
    private float m_sizeColums = 25;
    [SerializeField]
    private float m_sizeRows = 10;
    private BoxCollider m_boxCollider;


    private readonly Color _normalColor = Color.grey;
    private readonly Color _selectedColor = Color.yellow;

    public float SizeColumns
    {
        get { return m_sizeColums; }
        set { m_sizeColums = value; }
    }

    public float SizeRows
    {
        get { return m_sizeRows; }
        set { m_sizeRows = value; }
    }

    private void GridFrameGizmo(float length, float width)
    {
        float cols;
        float rows;

        cols = (length / ((float)(gridSize) * scaleFactor)); 
        rows = (width / ((float)(gridSize) * scaleFactor));

        Gizmos.DrawLine(new Vector3(0, (float)height * scaleFactor, 0), new Vector3(0, (float)height, rows * (float)gridSize * scaleFactor));
        Gizmos.DrawLine(new Vector3(0, (float)height * scaleFactor, 0), new Vector3(cols * (float)gridSize * scaleFactor, (float)height * scaleFactor, 0));
        Gizmos.DrawLine(new Vector3(0, (float)height * scaleFactor, rows * (float)gridSize * scaleFactor), new Vector3(cols * (float)gridSize * scaleFactor, (float)height * scaleFactor, rows * (float)gridSize * scaleFactor));
        Gizmos.DrawLine(new Vector3(cols * (float)gridSize * scaleFactor, (float)height * scaleFactor, rows * (float)gridSize * scaleFactor), new Vector3(cols * (float)gridSize * scaleFactor, (float)height * scaleFactor, 0));       
    }

    private void GridGizmo(float cols, float rows, float length, float width)
    {
        for (int i = 1; i < cols; i++)
        {
            Gizmos.DrawLine(
                new Vector3(i * ((float)gridSize) * scaleFactor, (float)height * scaleFactor, 0), 
                new Vector3(i * (float)gridSize * scaleFactor, (float)height * scaleFactor, rows * (float)gridSize * scaleFactor));
        }
        for (int j = 1; j < rows ; j++)
        {
            Gizmos.DrawLine(new Vector3(0, (float)height * scaleFactor, j * (float)gridSize) * scaleFactor, new Vector3(cols * (float)gridSize * scaleFactor, (float)height * scaleFactor, j * (float)gridSize * scaleFactor));
        }
    }

    private void GridGizmo(float length, float width)
    {
        float cols;
        float rows;

        cols = (length / ((float)(gridSize) * scaleFactor));
        rows = (width / ((float)(gridSize) * scaleFactor));

        for (int i = 1; i < cols; i++)
        {
            Gizmos.DrawLine(
                new Vector3(i * ((float)gridSize) * scaleFactor, (float)height * scaleFactor, 0),
                new Vector3(i * (float)gridSize * scaleFactor, (float)height * scaleFactor, rows * (float)gridSize * scaleFactor));
        }
        for (int j = 1; j < rows; j++)
        {
            Gizmos.DrawLine(new Vector3(0, (float)height * scaleFactor, j * (float)gridSize) * scaleFactor, new Vector3(cols * (float)gridSize * scaleFactor, (float)height * scaleFactor, j * (float)gridSize * scaleFactor));
        }
    }

    public Vector3 WorldToGridCoordinates(Vector3 point)
    {
        Vector3 gridPoint = new Vector3(
            (float)((point.x - transform.position.x) / (float)gridSize) * scaleFactor, (float)height * scaleFactor,
            (float)((point.z - transform.position.z) / (float)gridSize) * scaleFactor);
        return gridPoint;
    }

    public Vector3 GridToWorldCoordinates(float col, float row)
    {
        Vector3 worldPoint = new Vector3(
            transform.position.x + (col * (float)gridSize) * scaleFactor, (float)height * scaleFactor,
            transform.position.z + (row * (float)gridSize) * scaleFactor);
        return worldPoint;
    }

    public bool IsInsideGridBounds(Vector3 point)
    {
        float cols;
        float rows;

        cols = m_sizeColums;
        rows = m_sizeRows; 

        float minX = transform.position.x;
        float maxX = minX + m_sizeColums;
        float minZ = transform.position.z;
        float maxZ = minZ + m_sizeColums;
        return (point.x >= minX && point.x <= maxX && point.z >= minZ && point.z <= maxZ);
    }

    public bool IsInsideGridBounds(float col, float row)
    {
        return (col >= 0 && col < (m_sizeColums) && row >= 0 && row < (m_sizeColums));
    }

    private void Awake()
    {
        if (Ins != null && Ins != this)
            DestroyImmediate(this);

        if (Ins == null)
            Ins = this;

        m_boxCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        if (Ins != null && Ins != this)
            DestroyImmediate(this);

        if (Ins == null)
            Ins = this;

        m_boxCollider = GetComponent<BoxCollider>();

    }

    public void Update()
    {
        transform.position = Vector3.zero;

        float cols = m_sizeColums; 
        float rows = m_sizeRows; 

        m_boxCollider.size = new Vector3(cols, 0f, rows);
        m_boxCollider.center = new Vector3(cols / 2f, (float)height, rows / 2f);
    }

    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = _normalColor;

        if ((float)gridSize == 0)
            return;
       
        GridGizmo(m_sizeColums, m_sizeRows);
        Gizmos.color = _selectedColor;
        GridFrameGizmo(m_sizeColums, m_sizeRows);
        Gizmos.color = oldColor;
    }

    [MenuItem("GameObject/3D Object/Custom Object")]
    public static void CreateObject()
    {
        GameObject go = Instantiate(Resources.Load("SnapToGridTest", typeof(GameObject))) as GameObject;
    }

}
