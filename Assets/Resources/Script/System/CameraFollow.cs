using UnityEngine;
using System.Linq;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    [SerializeField]
	private PolygonCollider2D range;
    [SerializeField]
    private float smooth;

    private PlayerControl player;

    int layerMask;

    void Start()
    {
		if (player == null) 
		{
            player = GameObject.FindObjectOfType<PlayerControl>();
		}
        layerMask = LayerMask.GetMask("Camera");
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (player != null)
        {
			Vector3 tmp = transform.position;
            if (range != null)
            {
				/*
                tmp.x = player.transform.position.x;
                
                if (!range.OverlapPoint(tmp))
                {
                    Vector2 direction = (tmp.x > transform.position.x) ? Vector2.left : Vector2.right;
                    RaycastHit2D hit = Physics2D.Raycast(tmp, direction, 100000, layerMask);
                    if (hit.collider == range)
                    {
                        tmp.x = hit.point.x;
                    }
                }

                tmp.y = player.transform.position.y;

                if (!range.OverlapPoint(tmp))
                {
                    Vector2 direction = (tmp.y > transform.position.y) ? Vector2.down : Vector2.up;
                    RaycastHit2D hit = Physics2D.Raycast(tmp, direction, 100000, layerMask);
                    if (hit.collider == range)
                    {
                        tmp.y = hit.point.y;
                    }
                }

                tmp.z = transform.position.z;
                if (range.OverlapPoint(tmp))
                {
                    transform.position = tmp;
                }
                else
                {
                    tmp.z = transform.position.z;
                }
                */
				tmp = player.transform.position;

				if (!range.OverlapPoint(tmp))
				{
					int index = 0;
					float distance = Mathf.Infinity;
					int nearest = 0;

					for (index = 0; index < range.points.Length; ++index) 
					{
						float d = Vector2.Distance (range.points [index], tmp);
						if (d < distance) 
						{
							distance = d;
							nearest = index;
						}
					}
            
                    Vector3 p1 = GetNearestPointToLineSegment(range.points[nearest], range.points[(nearest  - 1 + range.points.Length) % range.points.Length], player.transform.position);
                    Vector3 p2 = GetNearestPointToLineSegment(range.points[nearest], range.points[(nearest + 1) % range.points.Length], player.transform.position);

                    tmp = (Vector3.Distance(p1, tmp) > Vector3.Distance(p2, tmp)) ? p2 : p1;
                    Debug.DrawLine(tmp, player.transform.position);
				}
            }
            else
            {
                tmp = player.transform.position;
                tmp.z = transform.position.z;
                transform.position = tmp;
            }
        }
    }
	
	Vector3 GetNearestPointToLineSegment(Vector3 p1, Vector3 p2, Vector3 point)
	{
		Vector2 v1 = point - p1;
		Vector2 v2 = p2 - p1;
		if (Vector2.Angle (v1, v2) < 90f) 
		{
			v1 = point - p2;
			v2 = p1 - p2;
			if (Vector2.Angle (v1, v2) < 90f) 
			{
                return Vector3.Project(point, p2 - p1);
			}
		}

		float d1 = Vector2.Distance(p1, point);
        float d2 = Vector2.Distance(p2, point);

        return (d1 > d2) ? p2 : p1;
	}
    
}
