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
    private float speed;

    private bool isdead = false;

    private WireControl hitWire;
    private bool reached = false;

    private Rigidbody2D rb2d;       
    private WireControl wire1;
    private WireControl wire2;

    private GameObject PlayerImage;

	static private Vector3 revivePos;

    private Animator anim;

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

        if (!wire1.IsWiring || !wire2.IsWiring)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetBool("prepare", true);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!wire1.IsWiring)
            {
                wire1.ShootWire();
                anim.SetBool("prepare", false);
            }
            else if (!wire2.IsWiring)
            {
                wire2.ShootWire();
                anim.SetBool("prepare", false);
            }
        }
    }

    public void SetWireDestination(WireControl obj)
    {
        if (obj != null)
        {
            hitWire = obj;
            Vector2 direction = hitWire.Target.transform.position - transform.position;
            rb2d.AddForce(direction.normalized * speed);
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

        if (hitWire != null && !reached)
        {
            Vector2 direction = hitWire.Target.transform.position - transform.position;
            rb2d.AddForce(direction.normalized * speed);

            PlayerImage.transform.localScale = new Vector3((rb2d.velocity.x > 0 ? -1 : 1), 1, 1);
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
        anim.SetBool("move", false);
        reached = true;
        rb2d.velocity = Vector2.zero;
        if (hitWire == wire1)
        {
            hitWire = wire2;
            wire2.GrabWall(wall, point);
        }
        else if (hitWire == wire2)
        {
            hitWire = wire1;
            wire1.GrabWall(wall, point);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<BlueObject>() != null)
        {
            GrabWall(coll.gameObject, coll.contacts[0].point);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SkyblueObject obj = other.gameObject.GetComponent<SkyblueObject>();
        if (obj != null && hitWire.Target.transform.parent == other.transform)
        {
            anim.SetBool("move", false);
            reached = true;
            rb2d.velocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (!reached)
        {
            BlueObject wall = coll.gameObject.GetComponent<BlueObject>();
            if (wall != null && hitWire.Target.transform.parent == coll.transform)
            {
                GrabWall(wall.gameObject, coll.contacts[0].point);
            }
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
