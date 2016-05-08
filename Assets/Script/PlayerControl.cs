using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;             //Floating point variable to store the player's movement speed.
    public WireControl hitWire;
    public bool IsMoving { get { return !isStop; } }


    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private WireControl wire1;
    private WireControl wire2;
    private bool isStop = true;
    private Vector2 target;
    private SpriteRenderer body;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();

        wire1 = transform.GetChild(0).GetComponent<WireControl>();
        wire2 = transform.GetChild(1).GetComponent<WireControl>();
        body = GetComponent<SpriteRenderer>();
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

    public void SetTarget(Vector2 t)
    {
        target = t;
        isStop = false;
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (!isStop)
        {
            Vector2 direction = target - (Vector2)transform.position;
            if (direction.magnitude > 0.6)
            {
                rb2d.MovePosition(rb2d.position + direction.normalized * speed * Time.deltaTime);

                body.flipX = direction.x < 0;
            }
            else
            {
                isStop = true;
            }
        }
    }
}
