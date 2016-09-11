using UnityEngine;
using System.Linq;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    [SerializeField]
	private PolygonCollider2D range;
    [SerializeField]
    private float smooth;

    private PlayerControl player;

    void Start()
    {
		if (player == null) 
		{
            player = GameObject.FindObjectOfType<PlayerControl>();
		}
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (player != null)
        {
			Vector3 tmp = transform.position;
            if (range != null)
            {
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

                    Debug.DrawLine(p1, player.transform.position, Color.white);
                    Debug.DrawLine(p2, player.transform.position, Color.white);
                    Debug.DrawLine(tmp, player.transform.position, Color.red);
				}
            }
            else
            {
                tmp = player.transform.position;
                
            }
            tmp.z = transform.position.z;
            transform.position = tmp;
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
                Vector2 vector = point - p1;
                Vector2 onNormal = p2 - p1;

                return p1 + Vector3.Project(vector, onNormal);
            }
		}

		float d1 = Vector2.Distance(p1, point);
        float d2 = Vector2.Distance(p2, point);

        return (d1 > d2) ? p2 : p1;
	}
    
}
