using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OnceInGame : MonoBehaviour {
    [SerializeField]
    private int guid;

	public void SetGuid(int v)
    {
        guid = v;
    }

    public void BeDestroyed()
    {
        OnceObjectManager.RegisterObject(SceneManager.GetActiveScene().name, guid);
    }
}
