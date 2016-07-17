using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ReviveControl : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

	void OnTriggerEnter2D(Collider2D other) 
	{
        PlayerControl player = other.gameObject.GetComponent<PlayerControl>();
        if (player != null) 
		{
            player.SetRevivePos (transform.position);
		}
	}
}
