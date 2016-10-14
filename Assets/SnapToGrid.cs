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


    private void Update()
    {
        //BoxCollider boxCollider = GetComponent<BoxCollider>();
        //boxCollider.enabled = EditorApplication.isPlaying; 
    }

    void OnMouseOver()
    {
        Debug.Log(name); 
    }

}
