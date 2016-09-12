using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartStage : MonoBehaviour {
    [SerializeField]
    private string sceneName;
	[SerializeField]
	private float waitTime = 4f;

	void Start()
	{
        EndingStage.ClearCoin();
        OnceObjectManager.ClearRegister();
        StartCoroutine(OP());
    }

	IEnumerator OP()
	{
		yield return new WaitForSeconds(waitTime);
        while (Handheld.PlayFullScreenMovie("op.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput));
		SceneManager.LoadScene(sceneName);
	}
}