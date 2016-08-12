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
    private float reachedDistance;

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

        reachedDistance = GetComponent<CircleCollider2D>().radius;

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
            //if (reached)
            //{
            //    transform.position = hitWire.Target.transform.position;
            //}
            //else if (   rb2d.velocity == Vector2.zero || 
            //            Vector3.Distance(hitWire.Target.transform.position, transform.position) < reachedDistance)
            //{
            //    anim.SetBool("move", false);
            //    reached = true;
            //}
            //else
            //{
            //    Vector2 direction = hitWire.Target.transform.position - transform.position;
            //    rb2d.velocity = direction.normalized * speed;
            //    PlayerImage.transform.localScale = new Vector3((direction.x > 0 ? -1 : 1), 1, 1);
            //}

            Vector2 direction = hitWire.Target.transform.position - transform.position;
            rb2d.AddForce(direction.normalized * speed);

            PlayerImage.transform.localScale = new Vector3((rb2d.velocity.x > 0 ? -1 : 1), 1, 1);
        }
    }
	
    public void Die()
    {
        if (isdead)
        {
            anim.SetTrigger("die");
            isdead = true;
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
