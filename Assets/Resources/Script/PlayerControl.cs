using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public WireControl HitWire { get { return hitWire; } }

    [SerializeField]
    private float speed;
    [SerializeField]
    private float reachRadius;

    private WireControl hitWire;
    private bool reached = false;

    private Rigidbody2D rb2d;       
    private WireControl wire1;
    private WireControl wire2;

    private GameObject PlayerImage;


    void Start()
    {
        
        rb2d = GetComponent<Rigidbody2D>();

        wire1 = transform.GetChild(0).GetComponent<WireControl>();
        wire2 = transform.GetChild(1).GetComponent<WireControl>();
        PlayerImage = transform.FindChild("PlayerImage").gameObject;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (wire1.IsWiring)
            {
                wire2.ShootWire();
            }
            else
            {
                wire1.ShootWire();
            }
        }
    }

    public void SetWireDestination(WireControl obj)
    {
        hitWire = obj;
        reached = false;
    }

    void FixedUpdate()
    {
        if (hitWire != null && hitWire.WireDestination != null &&
            hitWire.WireDestination.transform.parent.gameObject.tag == "Grabbable")
        {
            if (reached)
            {
                transform.position = hitWire.WireDestination.transform.position;
            }
            else
            {

                Vector2 direction = hitWire.WireDestination.transform.position - transform.position;
                if (direction.magnitude < reachRadius)
                {
                    reached = true;
                }
                else
                {
                    rb2d.MovePosition(rb2d.position + direction.normalized * speed * Time.deltaTime);
                    PlayerImage.transform.localScale = new Vector3((direction.x > 0 ? -1 : 1), 1, 1);
                }
            }
        }
    }
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "enemy") {
			Destroy (gameObject);
		}

	}

			
}
