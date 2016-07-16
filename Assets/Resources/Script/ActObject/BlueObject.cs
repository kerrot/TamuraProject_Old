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

        if (c != null && c.OverlapPoint(wire.Target.transform.position))
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
}
