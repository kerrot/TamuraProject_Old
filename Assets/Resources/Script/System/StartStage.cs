using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartStage : MonoBehaviour {
    [SerializeField]
    private string sceneName;
	[SerializeField]
	private float waitTime = 3f;

	bool start = true;

	void Start()
	{
        EndingStage.ClearCoin();
    }

	void Update () 
	{
		if (start) {
			start = false;
			StartCoroutine (OP());
		}
	}

	IEnumerator OP()
	{
		yield return new WaitForSeconds(waitTime);
		Handheld.PlayFullScreenMovie("op.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
		SceneManager.LoadScene(sceneName);
	}
}