using UnityEngine;
using System.Collections;

public class MushiBody : MonoBehaviour {
    public GameObject Follow;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 offset;
    private float distance;

	void Start () {
        RecordFollow();
        distance = (lastPosition - transform.position).magnitude;
    }
	
    void FixedUpdate()
    {
        transform.position = lastPosition - offset.normalized * distance;
        transform.rotation = lastRotation;

        RecordFollow();
    }

    void RecordFollow()
    {
        if (Follow != null)
        {
            lastPosition = Follow.transform.position;
            lastRotation = Follow.transform.rotation;
            offset = Follow.transform.position - transform.position;
        }
    }
}
