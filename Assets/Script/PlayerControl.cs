using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;             
    public WireControl hitWire;

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

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (hitWire != null && hitWire.WireDestination != null &&
            hitWire.WireDestination.transform.parent.gameObject.tag == "Blue")
        {
            Vector2 direction = hitWire.WireDestination.transform.position - transform.position;
            rb2d.MovePosition(rb2d.position + direction.normalized * speed * Time.deltaTime);
            PlayerImage.transform.localScale = new Vector3((direction.x > 0 ? -1 : 1), 1, 1);
        }
    }
}
