using UnityEngine;
using System.Collections;

public class WireControl : MonoBehaviour {

	[SerializeField]
	private float WireMaxLength = 0;
	[SerializeField]
	private float WireTime = 1;

    public bool IsWiring { get { return line.enabled; } }
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

    void Awake ()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        layerMask = 1 << LayerMask.NameToLayer("Hittable");
        player = transform.parent.gameObject;
        playerCtrl = player.GetComponent<PlayerControl>();
        Hand = transform.FindChild("Hand").gameObject;
        WireTarget = transform.FindChild("WireTarget").gameObject;
        minHitDistance = player.GetComponent<CircleCollider2D>().radius;
        minHitDistance = (WireMaxLength > minHitDistance) ? minHitDistance : 0;
    }

    public void ShootWire()
    {
        if (!line.enabled)
        {
            LineReset();

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            wireStep = ((Vector3)pos - WireTarget.transform.position).normalized;

            line.enabled = true;
        }
    }

    void SetHandRotation(Vector2 v)
    {
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Hand.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
    }

    void FixedUpdate()
    {
        if (playerCtrl.IsDead)
        {
            return;
        }

        if (line.enabled)
        {
            if (isReached)
            {
                if (playerCtrl.HitWire != this)
                {
                    Vector3 direction = WireTarget.transform.position - transform.position;
                    float step = Time.deltaTime / WireTime * WireMaxLength;
                    if (direction.magnitude <= step)
                    {
                        line.enabled = false;
                        return;
                    }
                    WireTarget.transform.position -= direction.normalized * Time.deltaTime / WireTime * WireMaxLength;
                }
            }
            else
            {
                WireTarget.transform.position += wireStep * Time.deltaTime / WireTime * WireMaxLength;
                if (Vector3.Distance(WireTarget.transform.position, transform.position) > minHitDistance)
                {
                    if (WireHitSomething())
                    {
                        isReached = true;
                    }
                    else if (Vector3.Distance(WireTarget.transform.position, transform.position) >= WireMaxLength)
                    {
                        isReached = true;
                    }
                }
            }

            DrawWire();
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
                    return true;
                }
            }
        }

        return false;
    }
}
