using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class MusiControl : EnemyControl {

    public GameObject Destination { get { return destination; } }

    protected GameObject destination;
}
