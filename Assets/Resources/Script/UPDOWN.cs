using UnityEngine;
using System.Collections;

public class /*オブジェクトの名前*/UPDOWN : MonoBehaviour {

	private Vector3 initialPosition;

	void Start () {
		//距離を測るため
		initialPosition = transform.position;

	}

	void Update () {

		transform.position = new Vector3(initialPosition.x, Mathf.Sin(Time.time) * 2.0f + initialPosition.y, initialPosition.z);

		//transform.position = new Vector3(initialPosition.x, Mathf.Sin(Time.time) * /*移動距離*/2.0f +  initialPosition.y, initialPosition.z);



	}
}