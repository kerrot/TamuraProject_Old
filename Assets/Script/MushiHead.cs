using UnityEngine;
using System.Collections;

public class MushiHead : MonoBehaviour {
    public GameObject Target;
    
    private NavMeshAgent navi;

	// Use this for initialization
	void Start () {
        navi = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        navi.SetDestination(Target.transform.position);
	}
}