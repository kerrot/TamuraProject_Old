using UnityEngine;
using System.Collections;

public class MoviePlay : MonoBehaviour {
	[SerializeField]
	private float second;

	private float last;

	void Start()
	{
		LaunchProjectile ();
	}

	void Update () 
	{
		if (Time.time - last > second) 
		{
			LaunchProjectile ();
		}
	}

	void LaunchProjectile ()
	{
		Handheld.PlayFullScreenMovie("qwe.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
		last = Time.time;
	}

	void OnGUI(){
		if (GUILayout.Button("オープンニング", GUILayout.Height(150)))
		{
			LaunchProjectile ();
		}
	}


}