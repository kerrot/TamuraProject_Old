using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoadScript: MonoBehaviour {

	public void SceneLoad (){
        SceneManager.LoadScene("DemoStage");
	} 
}
