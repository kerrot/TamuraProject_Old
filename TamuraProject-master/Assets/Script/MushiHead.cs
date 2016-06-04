using UnityEngine;
using System.Collections;

public class MushiHead : MonoBehaviour {
    [SerializeField]
    private PathEditor editor;
    [SerializeField]
    private float speed;

    GameObject destination;

    void Start()
    {
        editor.OnPathLoaded += Editor_OnPathLoaded;
    }

    private void Editor_OnPathLoaded()
    {
        FindNearestDestination();
    }

    void FixedUpdate()
    {
        Vector3 step = (destination.transform.position - transform.position).normalized * speed * Time.deltaTime;
        if ((destination.transform.position - transform.position).magnitude < step.magnitude)
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
            int index = Random.Range(0, point.ConnectPoint.Count);
            destination = point.ConnectPoint[index].Base;
        }
        else
        {
            FindNearestDestination();
        }
    }
}