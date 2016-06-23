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
        FindNearestDestination();
        transform.LookAt(destination.transform);

        foreach (var b in bodys)
        {
            b.InitDestination();
        }
    }

    void FixedUpdate()
    {
        Vector3 step = (destination.transform.position - transform.position).normalized * speed * Time.deltaTime;
        step.z = 0;
        if (step.magnitude == 0 || Vector3.Distance(destination.transform.position, transform.position) <= step.magnitude)
        {
            transform.position = destination.transform.position;
            FindNextDestination();

            transform.LookAt(destination.transform);
        }
        else
        {
            transform.position += step;
        }
    }

    void FindNearestDestination()
    {
        lastDestination = destination;

        editor.Paths.ForEach(p =>
        {
            if (p.from != null && p.to !=null)
            {
                float fromDistance = Vector3.Distance(p.from.transform.position, transform.position);
                float toDistance = Vector3.Distance(p.to.transform.position, transform.position);

                GameObject closer = (fromDistance > toDistance) ? p.to.gameObject : p.from.gameObject;

                if (destination == null ||
                    Vector3.Distance(closer.transform.position, transform.position) < Vector3.Distance(destination.transform.position, transform.position))
                {
                    destination = closer;
                }
            }
        });
    }

    void FindNextDestination()
    {
        var paths = editor.Paths.FindAll(p => p.from.gameObject == destination || p.to.gameObject == destination);
        if (paths.Count > 0)
        {
            if (paths.Count > 1)
            {
                while (true)
                {
                    int index = Random.Range(0, paths.Count);
                    if (paths[index].from.gameObject != lastDestination && paths[index].to.gameObject != lastDestination)
                    {
                        lastDestination = destination;
                        destination = (paths[index].from.gameObject == destination) ? paths[index].to.gameObject : paths[index].from.gameObject;
                        return;
                    }
                }
            }
            else
            {
                lastDestination = destination;
                destination = (paths[0].from.gameObject == destination) ? paths[0].to.gameObject : paths[0].from.gameObject;
            }
        }
    }
}