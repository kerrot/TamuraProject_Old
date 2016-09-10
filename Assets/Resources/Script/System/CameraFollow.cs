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
				/*Vector3 dest = player.transform.position;

				if (!range.OverlapPoint(dest))
				{
					int index = 0;
					float distance = Mathf.Infinity;
					int nearest = null;

					for (index = 0; index < range.points.Length; ++index) 
					{
						float tmp = Vector2.Distance (range.points [i], dest);
						if (tmp < distance) 
						{
							distance = tmp;
							nearest = index;
						}
					}


				}*/
            }
            else
            {
                tmp = player.transform.position;
                tmp.z = transform.position.z;
                transform.position = tmp;
            }
        }
    }
	/*
	float GetDistanceToLineSegment(Vector2 p1, Vector2 p2, Vector2 point)
	{
		Vector2 v1 = point - p1;
		Vector2 v2 = p2 - p1;
		if (Vector2.Angle (v1, v2) < 90) 
		{
			v1 = point - p2;
			v2 = p1 - p2;
			if (Vector2.Angle (v1, v2) < 90) 
			{

			}
		}

		float d1;
	}
    */
}
