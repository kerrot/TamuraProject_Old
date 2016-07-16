using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MushiBody : MusiControl
{
    [SerializeField]
    private MusiControl follow;

	bool isInit = false;

	private float speed;

	public void InitDestination(float s)
    {
		if (isInit) 
		{
			return;
		}

		speed = s;

		if (follow.Destination == null) 
		{
			MushiBody tmp = follow as MushiBody;
			if (tmp != null) 
			{
				tmp.InitDestination (speed);
			}
		}

        destination = follow.Destination;

        transform.LookAt(destination.transform);

        isInit = true;
    }

    void FixedUpdate()
    {
		if (!isInit) 
		{
			return;
		}

		Vector3 step = transform.forward * Time.deltaTime * speed;
		if (Vector3.Distance (transform.position, destination.transform.position) < step.magnitude) {
			transform.position = destination.transform.position;
			destination = follow.Destination;
			transform.LookAt (destination.transform);

		} 
		else 
		{
			transform.position += step;
		}
    }

    //override protected void ()
    //{
    //    follow.Attacked();
    //}
}
