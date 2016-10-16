using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.EventSystems;


[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{


    private void Awake()
    {
#if !UNITY_EDITOR
        Destroy(this); 
#endif

    }

}
