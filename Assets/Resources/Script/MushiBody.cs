using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MushiBody : MusiControl
{
    [SerializeField]
    private MusiControl follow;

    float distance;
	bool isInit = false;

    void Start()
    {
        distance = (follow.transform.position - transform.position).magnitude;
    }

    public void InitDestination()
    {
		if (isInit) 
		{
			return;
		}

		if (follow.Destination == null) 
		{
			MushiBody tmp = follow as MushiBody;
			if (tmp != null) 
			{
				tmp.InitDestination ();
			}
		}

        destination = follow.Destination;

        transform.rotation = Quaternion.LookRotation(destination.transform.position - transform.position);

		isInit = true;
    }

    void FixedUpdate()
    {
		if (!isInit) 
		{
			return;
		}

        if (follow.Destination == destination)
        {
            transform.position = follow.transform.position - follow.transform.forward * distance;
        }
        else if ((follow.transform.position - destination.transform.position).magnitude >= distance)
        {
            // notice that the distance of two destinations larger than value 'distance'
            destination = follow.Destination;
            
            transform.position = follow.transform.position - follow.transform.forward * distance;

            transform.rotation = Quaternion.LookRotation(destination.transform.position - transform.position);
        }
        else
        {
            Vector3 step = transform.forward * Time.deltaTime;
            float todestination = (destination.transform.position - transform.position).magnitude;
            float tofollow = (follow.transform.position - (transform.position + step)).magnitude;
            while (step.magnitude < todestination && tofollow > distance)
            {
                step += transform.forward * Time.deltaTime;
                tofollow = (follow.transform.position - (transform.position + step)).magnitude;
            }
            transform.position += step;
        }
    }
}
