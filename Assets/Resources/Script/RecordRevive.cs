using UnityEngine;
using System.Collections;

public class RecordRevive : MonoBehaviour {

	void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
