using UnityEngine;
using System.Collections;

public class MoviePlay : MonoBehaviour {

	void Start ()
	{
		((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
	}
}
