using UnityEngine;
using System.Collections;

public class MoviePlay : MonoBehaviour {

	void Start ()
	{
#if !UNITY_IOS
        ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
#endif
    }
}
