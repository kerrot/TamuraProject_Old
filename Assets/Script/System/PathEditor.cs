using UnityEngine;
using System;
using System.Collections.Generic;

public class PathEditor : MonoBehaviour {
    [SerializeField]
    private float displaySize = 1;

    public static float DisplaySize;

    [Serializable]
    public struct LinkedPath
    {
        public PathPointControl from;
        public PathPointControl to;
    }
    public List<LinkedPath> Paths = new List<LinkedPath>();

    void OnDrawGizmos()
    {
        DisplaySize = displaySize;

        Gizmos.color = Color.black;
        foreach (LinkedPath p in Paths)
        {
            if (p.from != null && p.to != null)
            {
                Gizmos.DrawLine(p.from.transform.position, p.to.transform.position);
            }
        }
    }
}
