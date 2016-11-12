using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MushiBody : MusiControl
{
    [SerializeField]
    private MusiControl follow;

    float offset;

	void Start()
    {
        offset = Vector3.Distance(follow.transform.position, transform.position);
    }

    void FixedUpdate()
    {
        Vector3 direction = follow.transform.position - transform.position;
        if (direction.magnitude > offset)
        {
            transform.position += direction.normalized * (direction.magnitude - offset);

            transform.LookAt(follow.transform);
        }
    }

    override public bool IsHitted(WireControl wire, RaycastHit2D hit)
    {
        Action(wire, hit);
        follow.IsHitted(wire, hit);

        return true;
    }
}
