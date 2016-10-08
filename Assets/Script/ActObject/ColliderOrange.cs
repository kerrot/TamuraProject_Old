using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ColliderOrange : ColliderEnter
{
    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Green")
        {
            Action(coll.gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Green")
        {
            Action(other.gameObject);
        }
    }
}