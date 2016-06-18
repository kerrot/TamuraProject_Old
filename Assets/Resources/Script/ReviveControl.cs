using UnityEngine;
using System.Collections;

public class ReviveControl : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Player") 
		{
			GameObject.FindObjectOfType<PlayerControl> ().SetRevivePos (transform.position);
		}
	}
}
