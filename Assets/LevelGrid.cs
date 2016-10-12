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
    public Pow2 height;
    public float multiplicationValue = 1f; 

    [HideInInspector]
    public GameObject selectedGameObject; 

    [SerializeField]
    private int m_sizeColums = 25;
    [SerializeField]
    private int m_sizeRows = 10;
    //[SerializeField]
    private int m_gridMultiplier = 128; 

    private int m_realSizeColums;
    private int m_realSizeRows;

    //private GameObject m_selectedGameObject; 
    private BoxCollider m_boxCollider;


    private readonly Color _normalColor = Color.grey;
    private readonly Color _selectedColor = Color.yellow;

    public int SizeColumns
    {
        get { return m_realSizeColums; }
        set { m_sizeColums = value; }
    }

    public int SizeRows
    {
        get { return m_realSizeRows; }
        set { m_sizeRows = value; }
    }

    private void GridFrameGizmo(int cols, int rows)
    {
        Gizmos.DrawLine(new Vector3(0, (int)height, 0), new Vector3(0, (int)height, rows * (int)gridSize));
        Gizmos.DrawLine(new Vector3(0, (int)height, 0), new Vector3(cols * (int)gridSize, (int)height, 0));
        Gizmos.DrawLine(new Vector3(cols * (int)gridSize, (int)height, 0), new Vector3(cols * (int)gridSize, (int)height, rows * (int)gridSize));
        Gizmos.DrawLine(new Vector3(0, (int)height, rows * (int)gridSize), new Vector3(cols * (int)gridSize, (int)height, rows * (int)gridSize));
    }

    private void GridGizmo(int cols, int rows)
    {
        for (int i = 1; i < cols; i++)
        {
            Gizmos.DrawLine(new Vector3(i * (int)gridSize, (int)height, 0), new Vector3(i * (int)gridSize, (int)height, rows * (int)gridSize));
        }
        for (int j = 1; j < rows; j++)
        {
            Gizmos.DrawLine(new Vector3(0, (int)height, j * (int)gridSize), new Vector3(cols * (int)gridSize, (int)height, j * (int)gridSize));
        }
    }

    public Vector3 WorldToGridCoordinates(Vector3 point)
    {
        Vector3 gridPoint = new Vector3(
            (int)((point.x - transform.position.x) / (int)gridSize), (int)height,
            (int)((point.z - transform.position.z) / (int)gridSize));
        return gridPoint;
    }

    public Vector3 GridToWorldCoordinates(int col, int row)
    {
        Vector3 worldPoint = new Vector3(
            transform.position.x + (col * (int)gridSize), (int)height,
            transform.position.z + (row * (int)gridSize));
        return worldPoint;
    }

    public Vector3 GridToWorldCoordinates(float col, float row)
    {
        Vector3 worldPoint = new Vector3(
            transform.position.x + (col * (int)gridSize + (int)gridSize / 2.0f), (int)height,
            transform.position.z + (row * (int)gridSize + (int)gridSize / 2.0f));
        return worldPoint;
    }

    public bool IsInsideGridBounds(Vector3 point)
    {
        float minX = transform.position.x;
        float maxX = minX + m_realSizeColums * (int)gridSize;
        float minZ = transform.position.z;
        float maxZ = minZ + m_realSizeRows * (int)gridSize;
        return (point.x >= minX && point.x <= maxX && point.z >= minZ && point.z <= maxZ);
    }

    public bool IsInsideGridBounds(int col, int row)
    {
        return (col >= 0 && col < m_realSizeColums && row >= 0 && row < m_realSizeRows);
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
        m_gridMultiplier = (int)gridSize; 
        m_realSizeColums = m_sizeColums * m_gridMultiplier;
        m_realSizeRows = m_sizeRows * m_gridMultiplier;

        m_boxCollider.size = new Vector3(m_realSizeColums, 0f, m_realSizeRows);
        m_boxCollider.center = new Vector3(m_realSizeColums / 2f, (int)height, m_realSizeRows / 2f);
    }

    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = _normalColor;

        if ((int)gridSize == 0)
            return;

        GridGizmo(m_realSizeColums / (int)gridSize, m_realSizeRows / (int)gridSize);
        Gizmos.color = _selectedColor;
        GridFrameGizmo(m_realSizeColums / (int)gridSize, m_realSizeRows / (int)gridSize);
        Gizmos.color = oldColor;
    }

    private void OnDrawGizmosSelected()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = _selectedColor;

        if ((int)gridSize == 0)
            return;

        GridFrameGizmo(m_realSizeColums / (int)gridSize, m_realSizeRows / (int)gridSize);
        Gizmos.color = oldColor;
    }

}
