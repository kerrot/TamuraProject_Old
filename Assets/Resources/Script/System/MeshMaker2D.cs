using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshMaker2D : MonoBehaviour
{
    [ContextMenu("Build")]
    void Start()
    {
        int pointCount = 0;
        PolygonCollider2D pc2 = gameObject.GetComponent<PolygonCollider2D>();
        pointCount = pc2.GetTotalPointCount();

        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        Vector2[] points = pc2.points;
        Vector3[] vertices = new Vector3[pointCount];
        Vector2[] uv = new Vector2[pointCount];
        for (int j = 0; j < pointCount; j++)
        {
            Vector2 actual = points[j];
            vertices[j] = new Vector3(actual.x, actual.y, 0);
            uv[j] = new Vector2(actual.x, actual.y);
        }
        Triangulator tr = new Triangulator(points);
        int[] triangles = tr.Triangulate();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mf.mesh = mesh;
    }
}
