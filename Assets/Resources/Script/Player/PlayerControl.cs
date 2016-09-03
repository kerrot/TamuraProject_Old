using UnityEngine;

using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour {

    public WireControl HitWire { get { return hitWire; } }

    public bool IsDead { get { return isdead; } }

    [SerializeField]
    private float acc;
    [SerializeField]
    private float startSpeed;
    [SerializeField]
    private float speedLimit;
    [SerializeField]
    private bool HitbyMouseDown;
    [SerializeField]
    private Animator bodyAnim;

    private Animator anim;

    private bool isdead = false;

    private WireControl hitWire;
    private bool reached = false;

    private Rigidbody2D rb2d;       
    private WireControl wireRight;
    private WireControl wireLeft;

    private GameObject PlayerImage;

	static private Vector3 revivePos;
    
    private Vector3 grabOffset;
    private float stopRadius;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        wireRight = transform.GetChild(0).GetComponent<WireControl>();
        wireLeft = transform.GetChild(1).GetComponent<WireControl>();
        PlayerImage = transform.FindChild("PlayerImage").gameObject;

        anim = GetComponent<Animator>();

        transform.position = revivePos;
        stopRadius = GetComponent<CircleCollider2D>().radius * transform.lossyScale.x;
    }

    void Update()
    {
        if (isdead)
        {
            return;
        }

        WireControl freeWire = GetFreeWire();

        if (freeWire != null)
        {
            if (HitbyMouseDown)
            {
                if (Input.GetMouseButton(0))
                {
                    freeWire.ShootWire();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    freeWire.PrepareShoot();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    freeWire.ShootWire();
                }
            }
        }

        if (rb2d.velocity.magnitude > speedLimit)
        {
            rb2d.velocity = rb2d.velocity.normalized * speedLimit;
        }
    }

    WireControl GetFreeWire()
    {
        if (!wireRight.IsWiring)
        {
            return wireRight;
        }

        if (!wireLeft.IsWiring)
        {
            return wireLeft;
        }

        return null;
    }

    public void SetWireDestination(WireControl obj)
    {
        if (obj != null)
        {
            if (obj == wireRight)
            {
                anim.SetTrigger("RightShoot");
            }
            else if (obj == wireLeft)
            {
                anim.SetTrigger("LeftShoot");
            }

            hitWire = obj;
            Vector2 direction = hitWire.Target.transform.position - transform.position;
            rb2d.velocity += direction.normalized * startSpeed;
            anim.SetFloat("Velocity", direction.x);
            anim.SetBool("Stop", false);
            reached = false;
        }
    }

    void FixedUpdate()
    {
        if (isdead)
        {
            return;
        }

        if (hitWire != null)
        {
            if (reached)
            {
                transform.position = hitWire.Target.transform.position + grabOffset;
            }
            else
            {
                Vector2 direction = hitWire.Target.transform.position - transform.position;
                rb2d.AddForce(direction.normalized * acc);

				//float angle = Vector2.Angle (Vector2.up, direction);
				//PlayerImage.transform.rotation = direction.x > 0 ? Quaternion.Euler(0, 0, -angle) : Quaternion.Euler(0, 0, angle);
	
                if (Vector3.Distance(hitWire.Target.transform.position, transform.position) < stopRadius)
                {
                    ReachWall();
                }
            }
        }
    }
	
    public void Die()
    {
        if (!isdead)
        {
            bodyAnim.SetTrigger("die");
            isdead = true;
        }
    }

    void GrabWall(GameObject wall, Vector2 point)
    {
        ReachWall();
        if (hitWire == wireRight)
        {
            wireRight.GrabWall(wall, point);
        }
        else if (hitWire == wireLeft)
        {
            wireLeft.GrabWall(wall, point);
        }

        grabOffset = transform.position - (Vector3)point;
    }

    void ReachWall()
    {
		//PlayerImage.transform.rotation = Quaternion.identity;
        anim.SetBool("Stop", true);
        reached = true;
        rb2d.velocity = Vector2.zero;
        grabOffset = transform.position - hitWire.Target.transform.position;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<BlueObject>() != null &&
            hitWire.Target.transform.parent == coll.transform)
        {
            GrabWall(coll.gameObject, coll.contacts[0].point);
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (!reached)
        {
            OnCollisionEnter2D(coll);
        }
    }

	public void SetRevivePos(Vector3 pos)
	{
        pos.z = 0;
        revivePos = pos;
	}
}
