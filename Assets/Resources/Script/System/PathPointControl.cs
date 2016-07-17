using UnityEngine;
//#if !UNITY_IOS
        using UnityEditor;
//#endif


public class PathPointControl : MonoBehaviour {

//#if !UNITY_IOS
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, PathEditor.DisplaySize);

        Handles.Label(transform.position + Vector3.up * 2, transform.name);
    }
//#endif
}
