using UnityEngine;
using UnityEngine.SceneManagement;
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
    private float stopRadius;
    [SerializeField]
    private bool HitbyMouseDown;


    private bool isdead = false;

    private WireControl hitWire;
    private bool reached = false;

    private Rigidbody2D rb2d;       
    private WireControl wire1;
    private WireControl wire2;

    private GameObject PlayerImage;

	static private Vector3 revivePos;

    private Animator anim;

    private Vector3 grabOffset;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        wire1 = transform.GetChild(0).GetComponent<WireControl>();
        wire2 = transform.GetChild(1).GetComponent<WireControl>();
        PlayerImage = transform.FindChild("PlayerImage").gameObject;

        anim = GetComponent<Animator>();

        transform.position = revivePos;
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
                    anim.SetTrigger("fire");
                    freeWire.ShootWire();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    anim.SetBool("prepare", true);
                }
                else if (anim.GetBool("prepare") && Input.GetMouseButtonUp(0))
                {
                    freeWire.ShootWire();
                    anim.SetBool("prepare", false);
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
        if (!wire1.IsWiring)
        {
            return wire1;
        }

        if (!wire2.IsWiring)
        {
            return wire2;
        }

        return null;
    }

    public void SetWireDestination(WireControl obj)
    {
        if (obj != null)
        {
            hitWire = obj;
            Vector2 direction = hitWire.Target.transform.position - transform.position;
            rb2d.velocity += direction.normalized * startSpeed;
            anim.SetBool("move", true);
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

                PlayerImage.transform.localScale = new Vector3((rb2d.velocity.x > 0 ? -1 : 1), 1, 1);

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
            anim.SetTrigger("die");
            isdead = true;
        }
    }

    void GrabWall(GameObject wall, Vector2 point)
    {
        ReachWall();
        if (hitWire == wire1)
        {
            wire1.GrabWall(wall, point);
        }
        else if (hitWire == wire2)
        {
            wire2.GrabWall(wall, point);
        }

        grabOffset = transform.position - (Vector3)point;
    }

    void ReachWall()
    {
        anim.SetBool("move", false);
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

    void ReTry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	public void SetRevivePos(Vector3 pos)
	{
		revivePos = pos;
	}
}
