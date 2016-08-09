#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
#define BEFORE_UNITY_4_3
#else
#define AFTER_UNITY_4_3
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Uni2DMeshShape2D
{
	public Vector2[] vertices;
	public Uni2DBoneWeight[] boneWeights;
	
	public BoneWeight[] GetBoneWeightStructs()
	{
		if(boneWeights == null)
		{
			return null;
		}
		
		int iBoneWeightCount = boneWeights.Length;
		
		BoneWeight[] oBoneWeights = new BoneWeight[iBoneWeightCount];
		for(int i = 0; i < iBoneWeightCount; ++i)
		{
			oBoneWeights[i] = boneWeights[i];
		}
		
		return oBoneWeights;
	}
}

[Serializable]
public class Uni2DMesh2D
{
	public List<Uni2DMeshShape2D> shapes;
	
	public Matrix4x4[] bindposes;
	
	public void ClearSkin()
	{
		bindposes = null;
		if(shapes != null)
		{
			foreach(Uni2DMeshShape2D rShape in shapes)
			{
				rShape.boneWeights = null;
			}
		}
	}

	#if AFTER_UNITY_4_3
	public void CopyVertices(PolygonCollider2D a_rPolygonCollider2DTo)
	{
		// Clear
		a_rPolygonCollider2DTo.points = new Vector2[0];
		
		if(shapes == null)
		{
			return;
		}
		
		a_rPolygonCollider2DTo.pathCount = shapes.Count;
		
		int iShapeIndex = 0;
		foreach(Uni2DMeshShape2D rShape in shapes)
		{
			a_rPolygonCollider2DTo.SetPath(iShapeIndex, rShape.vertices);
			++iShapeIndex;
		}
	}
	
	public static void CopyVertices(List<Vector2[]> a_rShapeVerticesFrom, PolygonCollider2D a_rPolygonCollider2DTo)
	{
		// Clear
		a_rPolygonCollider2DTo.points = new Vector2[0];
		
		a_rPolygonCollider2DTo.pathCount = a_rShapeVerticesFrom.Count;
		
		int iShapeIndex = 0;
		foreach(Vector2[] rVertices in a_rShapeVerticesFrom)
		{
			a_rPolygonCollider2DTo.SetPath(iShapeIndex, rVertices);
			++iShapeIndex;
		}
	}
	#endif
}