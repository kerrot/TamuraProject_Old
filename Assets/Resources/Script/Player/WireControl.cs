using UnityEngine;
using System.Collections;

public class WireControl : MonoBehaviour {

	[SerializeField]
	private float WireMaxLength = 0;
	[SerializeField]
	private float WireTime = 1;
    [SerializeField]
    private GameObject Head;

    public bool IsWiring { get { return line.enabled || isGrabbing; } }
    public GameObject Target { get { return WireTarget; } }

    private GameObject Hand;
    private GameObject WireTarget;
    

    private LineRenderer line;
    private int layerMask;

    private GameObject player;
    private PlayerControl playerCtrl;

    private Vector3 wireStep;
    private bool isReached = false;
    private float minHitDistance;

    private bool isGrabbing = false;

    private Animator anim;
    private AudioSource se;

    void Awake ()
    {
        line = GetComponentInChildren<LineRenderer>();
        line.enabled = false;
        layerMask = 1 << LayerMask.NameToLayer("Hittable");
        player = transform.parent.gameObject;
        playerCtrl = player.GetComponent<PlayerControl>();
        Hand = transform.FindChild("Hand").gameObject;
        WireTarget = transform.FindChild("WireTarget").gameObject;
        minHitDistance = player.GetComponent<CircleCollider2D>().radius;
        minHitDistance = (WireMaxLength > minHitDistance) ? minHitDistance : 0;
        anim = GetComponent<Animator>();
        se = player.GetComponent<AudioSource>();
    }

    public void ShootWire()
    {
        if (CanShoot())
        {
            LineReset();

            Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
            wireStep = ((Vector3)pos - WireTarget.transform.position).normalized;
            se.Play();
            line.enabled = true;
            Head.SetActive(true);

            if (anim.GetBool("prepare"))
            {
                anim.SetBool("prepare", false);
            }
            else
            {
                anim.SetTrigger("shoot");
            }

        }
    }

    public void PrepareShoot()
    {
        if (CanShoot())
        {
            anim.SetBool("prepare", true);
        }
    }

    bool CanShoot()
    {
        return !line.enabled && !isGrabbing;
    }

    public void GrabWall(GameObject wall, Vector2 point)
    {
        if (playerCtrl.HitWire == this)
        {
            isGrabbing = true;
            isReached = true;
            WireTarget.transform.parent = wall.transform;
            WireTarget.transform.position = point;
        }
    }

    void SetHandRotation(Vector2 v)
    {
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Hand.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        WireTarget.transform.rotation = Hand.transform.rotation;
    }

    void FixedUpdate()
    {
        if (playerCtrl.IsDead)
        {
            return;
        }

        if (line.enabled)
        {
            float step = Time.deltaTime / WireTime * WireMaxLength;
            if (isReached)
            {
                if (playerCtrl.HitWire != this)
                {
                    Vector3 direction = WireTarget.transform.position - transform.position;
                    
                    if (direction.magnitude <= step)
                    {
                        line.enabled = false;
                        Head.SetActive(false);
                        return;
                    }
                    WireTarget.transform.position -= direction.normalized * step;
                }
            }
            else
            {
                WireTarget.transform.position += wireStep * step;
                if (Vector3.Distance(WireTarget.transform.position, transform.position) > minHitDistance)
                {
                    if (WireHitSomething())
                    {
                        isReached = true;
                    }
                    else if (Vector3.Distance(WireTarget.transform.position, transform.position) >= WireMaxLength)
                    {
                        anim.SetTrigger("hit");
                        isReached = true;
                    }
                }
            }

            DrawWire();
        }

        if (isGrabbing)
        {
            SetHandRotation(WireTarget.transform.position - player.transform.position);

            if (playerCtrl.HitWire != this)
            {
                isGrabbing = false;
            }
        }
    }

    void LineReset()
    {
        WireTarget.transform.parent = null;
        WireTarget.transform.position = transform.position;
        isReached = false;
    }

    void DrawWire()
    {
        Vector2 tmp = (Vector2)(WireTarget.transform.position - player.transform.position);
        line.SetPosition(0, player.transform.position);
        line.SetPosition(1, WireTarget.transform.position);
        line.material.mainTextureScale = new Vector2(tmp.magnitude, 1);

        SetHandRotation(tmp);
    }

    bool WireHitSomething()
    {
        Vector3 direction = WireTarget.transform.position - transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, direction.magnitude, layerMask);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                HittableObject obj = hit.collider.gameObject.GetComponent<HittableObject>();
                if (obj != null && obj.IsHitted(this, hit))
                {
                    anim.SetTrigger("hit");
                    return true;
                }
            }
        }

        return false;
    }
}
