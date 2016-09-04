using UnityEngine;
using System.Collections;

public class SpiderControl : MonoBehaviour {
    [SerializeField]
    private float downSpeed;
    [SerializeField]
    private float upSpeed;


    private PlayerControl player;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindObjectOfType<PlayerControl>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 tmp = transform.position;

        if (player.transform.position.y > transform.position.y)
        {
            float diff = player.transform.position.y - transform.position.y;
            tmp.y += (diff > upSpeed) ? upSpeed : diff;
            transform.position = tmp;
        }
        else
        {
            float diff = transform.position.y - player.transform.position.y;
            tmp.y -= (diff > downSpeed) ? downSpeed : diff;
        }

        transform.position = tmp;
    }
}
