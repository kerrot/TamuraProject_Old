using UnityEngine;
using UnityEditor;

public class PathPointControl : MonoBehaviour {
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, PathEditor.DisplaySize);

        Handles.Label(transform.position + Vector3.up * 2, transform.name);
    }
}
