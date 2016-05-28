using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MushiBody : MonoBehaviour {
	[SerializeField]
	private GameObject Follow;

	private float distance;
	private Rigidbody bdy;
	private List<Vector3> records = new List<Vector3>();

	void Start () {
		bdy = GetComponent<Rigidbody> ();

		distance = (Follow.transform.position - transform.position).magnitude;

		records.Add (Follow.transform.position);
    }
	
    void FixedUpdate()
    {
		transform.position += records [0] - transform.position;
		if (transform.position == records [0]) {
			records.RemoveAt (0);
		}

		records.Add (Follow.transform.position);
    }
}
