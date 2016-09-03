using UnityEngine;
using System.Collections;

public class TracePlayerBehavior : MonoBehaviour {
    [SerializeField]
    private float speed;
    [SerializeField]
    private float checkTime;
    [SerializeField]
    private float checkRadius;
    [SerializeField]
    private bool AlsoRotate;

    private bool checking = true;
    private PlayerControl player;
    private Vector3 direction;
    private SpriteRenderer sprite;

	private bool moving = false;

    void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerControl>();
        direction = transform.right;
    }

    void Update()
    {
        if (checking)
        {
			if (Vector3.Distance (transform.position, player.transform.position) < checkRadius) {
				ChangeDirection ();
				moving = true;
			} 
			else {
				moving = false;
			}
        }
    }

    void FixedUpdate()
    {
		if (moving) {
			transform.position += direction * Time.deltaTime * speed;
		}
    }

    void ChangeDirection()
    {
        checking = false;
        StartCoroutine(ReCheck());

        direction = (player.transform.position - transform.position).normalized;

        if (AlsoRotate)
        {
            transform.rotation = Quaternion.Euler(0, 0, ((direction.y > 0) ? 1 : -1) * Vector2.Angle(Vector2.right, direction));
        }
        else
        {
            transform.localScale = new Vector3((direction.x > 0 ? -1 : 1) * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator ReCheck()
    {
        yield return new WaitForSeconds(checkTime);
        if (Vector3.Distance(transform.position, player.transform.position) < checkRadius)
        {
            ChangeDirection();
        }
        else
        {
            checking = true;
        }
    }
}
