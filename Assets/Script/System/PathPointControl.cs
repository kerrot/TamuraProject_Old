using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class PathPointControl : MonoBehaviour {

    void OnDrawGizmos()
    {
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(transform.position, PathEditor.DisplaySize);
#if UNITY_EDITOR

		Handles.Label(transform.position + Vector3.up * 2, transform.name);
#endif
	}
}
