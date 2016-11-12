using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartStage : MonoBehaviour {
    [SerializeField]
    private string sceneName;
	[SerializeField]
	private float waitTime = 4f;

    private float time;

	void Start()
	{
        EndingStage.ClearCoin();
        OnceObjectManager.ClearRegister();
        time = Time.time;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    void Update()
    {
        if (Time.time - time > waitTime)
        {
            Handheld.PlayFullScreenMovie("op.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
			time = Time.time;
        }
    }
}