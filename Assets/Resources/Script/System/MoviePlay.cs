using UnityEngine;
using System.Collections;

public class MoviePlay : MonoBehaviour {



	void Update () 
	{
		InvokeRepeating("LaunchProjectile",1,5);
	}

	void LaunchProjectile ()
	{
		Handheld.PlayFullScreenMovie("qwe.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
	}

	void OnGUI(){
		if (GUILayout.Button("オープンニング", GUILayout.Height(150)))
		{
			Handheld.PlayFullScreenMovie("qwe.mp4", Color.white, FullScreenMovieControlMode.CancelOnInput);
		}
	}


}