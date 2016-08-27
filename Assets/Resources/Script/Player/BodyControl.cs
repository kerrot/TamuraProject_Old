using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BodyControl : MonoBehaviour {

    void ReTry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
