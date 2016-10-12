using UnityEngine;
using System.Collections;
using UnityEditor; 

[CustomEditor(typeof(LevelGrid))]
public class LevelGridEditor : Editor
{

    LevelGrid _myTarget;
    

    private void OnEnable()
    {
        LevelGrid _myTarget = target as LevelGrid;
    }

    private void OnSceneGUI()
    {
        EventHandler(); 
    }

    private void EventHandler()
    {


    }



}
