using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MoviePlay : MonoBehaviour {
    [SerializeField]
    private string sceneName;

	void Start()
	{
        Handheld.PlayFullScreenMovie("op.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
    }

	void Update () 
	{
		if (Input.GetMouseButton(0)) 
		{
            SceneManager.LoadScene(sceneName);
		}
	}
}