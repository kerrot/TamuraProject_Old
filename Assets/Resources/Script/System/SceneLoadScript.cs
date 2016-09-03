using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoadScript: MonoBehaviour {

	public void SceneLoad (){
		SceneManager.LoadScene("GameClear");
	}

	public void SceneLoad1 (){
		SceneManager.LoadScene("masuta-sute-zi");
	}

	public void SceneLoad3 (){
		SceneManager.LoadScene("title");
	} 

}