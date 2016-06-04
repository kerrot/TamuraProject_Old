using UnityEngine;
using System.Collections;

public class MushiHead : MusiControl
{
    [SerializeField]
    private PathEditor editor;
    [SerializeField]
    private float speed;
	[SerializeField]
	private MushiBody[] bodys;
    
    GameObject lastDestination;

    void Awake()
    {
        editor.OnPathLoaded += Editor_OnPathLoaded;
    }

    private void Editor_OnPathLoaded()
    {
        FindNearestDestination();
		foreach (var b in bodys) 
		{
			b.InitDestination ();
		}
    }

    void FixedUpdate()
    {
        Vector3 step = (destination.transform.position - transform.position).normalized * speed * Time.deltaTime;
        step.z = 0;
        if (step.magnitude == 0 || (destination.transform.position - transform.position).magnitude <= step.magnitude)
        {
            transform.position = destination.transform.position;
            FindNextDestination();

            transform.rotation = Quaternion.LookRotation(destination.transform.position - transform.position);
        }
        else
        {
            transform.position += step;
        }
    }

    void FindNearestDestination()
    {
        lastDestination = destination;
        editor.Path.ForEach(r =>
        {
            if (destination == null)
            {
                destination = r.Base;
            }
            else
            {
                float distance = (r.Base.transform.position - transform.position).magnitude;
                float ori = (destination.transform.position - transform.position).magnitude;
                if (distance < ori)
                {
                    destination = r.Base;
                }
            }
        });
    }

    void FindNextDestination()
    {
        PathEditor.PathPoint point = editor.Path.Find(p => p.Base == destination);
        if (point != null && point.ConnectPoint.Count > 1)
        {
            while (true)
            {
                int index = Random.Range(0, point.ConnectPoint.Count);
                if (point.ConnectPoint[index].Base != lastDestination)
                {
                    lastDestination = destination;
                    destination = point.ConnectPoint[index].Base;
                    break;
                }
            }
        }
        else
        {
            FindNearestDestination();
        }
    }
}