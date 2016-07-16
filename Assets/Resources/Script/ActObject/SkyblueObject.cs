using UnityEngine;
using System.Collections;

public class SkyblueObject : BlueObject
{
    PlayerControl playerCtrl;

    override protected void Awake()
    {
        base.Awake();
        GetComponent<Collider2D>().isTrigger = true;
        playerCtrl = GameObject.FindObjectOfType<PlayerControl>();
    }

    override public bool IsHitted(WireControl wire, RaycastHit2D hit)
    {
        if (playerCtrl.HitWire == null || playerCtrl.HitWire.Target.transform.parent != hit.collider.gameObject.transform)
        {
            Grab(wire, hit);
            Action(wire, hit);

            return true;
        }


        return false;
    }
}
