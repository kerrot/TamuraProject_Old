using UnityEngine;
using System.Collections;

public class BlueObject : HittableObject
{
	override protected void Awake()
    {
        base.Awake();
        GetComponent<Collider2D>().isTrigger = false;
    }

    override public bool IsHitted(WireControl wire, RaycastHit2D hit)
    {
        Collider2D c = GetComponent<Collider2D>();

        if (CheckHit(wire))
        {
            Grab(wire, hit);
            Action(wire, hit);

            return true;
        }

        return false;
    }

    protected void Grab(WireControl wire, RaycastHit2D hit)
    {
        wire.Target.transform.parent = hit.collider.gameObject.transform;
        wire.Target.transform.position = hit.point;
        GameObject.FindObjectOfType<PlayerControl>().SetWireDestination(wire);
    }

    protected bool CheckHit(WireControl wire)
    {
        Collider2D c = GetComponent<Collider2D>();

        if (c != null)
        {
            Vector2 direction = wire.transform.position - wire.Target.transform.position;
            Vector2 step = direction.normalized * 0.1f;

            float length = direction.magnitude;
            Vector2 now = wire.Target.transform.position;
            while (Vector2.Distance(now, wire.Target.transform.position) <= length)
            {
                if (c.OverlapPoint(now))
                {
                    return true;
                }
                now += step;
            }
        }        

        return false;
    }
}
