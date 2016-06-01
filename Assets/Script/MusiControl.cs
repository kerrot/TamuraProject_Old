using UnityEngine;
using System;
using System.Collections;

public class MusiControl : MonoBehaviour {

    public GameObject Destination { get { return destination; } }

    protected GameObject destination;

    public delegate void MushiAction();
    public MushiAction OnDestinationSet;
}
