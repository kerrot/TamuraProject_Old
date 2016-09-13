using UnityEngine;
using System.Collections;

public class BossTraceCheck : MonoBehaviour {

	public bool PlayerInRange { get { return playerInRange; } }
    [SerializeField]
    private GameObject TraceEffect;

    private bool playerInRange = false;

	void OnTriggerEnter2D(Collider2D other) 
	{
		PlayerControl player = other.gameObject.GetComponent<PlayerControl>();
		if (player != null) 
		{
			playerInRange = true;
            TraceEffect.SetActive(true);
        }
	}

	void OnTriggerExit2D(Collider2D other) 
	{
		PlayerControl player = other.gameObject.GetComponent<PlayerControl>();
		if (player != null) 
		{
			playerInRange = false;
            TraceEffect.SetActive(false);
        }
	}
}
