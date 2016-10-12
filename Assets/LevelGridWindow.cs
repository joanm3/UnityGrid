using UnityEngine;
using UnityEditor; 
using System.Collections;

public class LevelGridWindow : EditorWindow
{
    LevelGrid m_levelGrid; 

    public void Init()
    {
        m_levelGrid = LevelGrid.Ins; 
    }

    void OnGUI()
    {
        m_levelGrid.gridSize = (LevelGrid.Pow2)EditorGUILayout.EnumPopup("Grid Size: ", m_levelGrid.gridSize);
        m_levelGrid.Update(); 
    }

}
