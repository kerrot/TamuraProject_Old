using UnityEngine;
using System.Collections;

public class WireControl : MonoBehaviour {

	[SerializeField]
	private float WireMaxLength;
	[SerializeField]
	private float WireTime;

    public bool IsWiring { get { return line.enabled; } }
    public GameObject WireDestination { get { return WireTarget; } }

    private GameObject Hand;
    private GameObject WireTarget;

    private LineRenderer line;
    private int layerMask;

    private bool isReached = false;
    private GameObject player;
    private PlayerControl playerCtrl;

    private Vector3 wireStep;

    void Start () {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        layerMask = 1 << LayerMask.NameToLayer("Hittable");
        player = transform.parent.gameObject;
        playerCtrl = player.GetComponent<PlayerControl>();
        Hand = transform.FindChild("Hand").gameObject;
        WireTarget = transform.FindChild("WireTarget").gameObject;
    }

    public void ShootWire()
    {
        if (!line.enabled)
        {
            LineReset();
            wireStep = ComputeScreenPosition().normalized;
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
        if (line.enabled)
        {
            if (isReached)
            {
                if (playerCtrl.hitWire != this)
                {
                    Vector3 distance = WireTarget.transform.position - transform.position;
                    float step = Time.deltaTime / WireTime * WireMaxLength;
                    if (distance.magnitude <= step)
                    {
                        line.enabled = false;
                        return;
                    }
                    WireTarget.transform.position -= distance.normalized * Time.deltaTime / WireTime * WireMaxLength;
                }
            }
            else
            {
                WireTarget.transform.position += wireStep * Time.deltaTime / WireTime * WireMaxLength;
                if (WireHitSomething())
                {
                    isReached = true;
                }
                else if ((WireTarget.transform.position - transform.position).magnitude >= WireMaxLength)
                {
                    isReached = true;
                }
            }

            DrawWire();
        }
    }

    void LineReset()
    {
        WireTarget.transform.parent = transform;
        WireTarget.transform.localPosition = new Vector3();
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

    Vector2 ComputeScreenPosition()
    {
		return Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;	
    }

//     Vector3 ComputeWireTarget()
//     {
//         Vector2 direction = ComputeScreenPosition();
//         Vector2 tmpTarget;
//         RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, WireMaxLength, layerMask);
// 
//         if (hit.collider != null)
//         {
// 
//             isHit = true;
//             tmpTarget = hit.point;
//         }
//         else
//         {
//             tmpTarget = (Vector2)player.transform.position + direction.normalized * WireMaxLength;
//             isHit = false;
//         }
// 
//         return new Vector3(tmpTarget.x, tmpTarget.y, 0);
//     }

    bool WireHitSomething()
    {
        Vector3 direction = WireTarget.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, direction.magnitude, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Blue")
            {
                WireTarget.transform.parent = hit.collider.gameObject.transform;
                WireTarget.transform.position = hit.point;
                playerCtrl.hitWire = this;
            }
            else if (hit.collider.gameObject.tag == "Orange")
            {
                OrangeController ctrl = hit.collider.gameObject.GetComponent<OrangeController>();
                if (ctrl != null)
                {
                    ctrl.TriggerAnim();
                }
            }

            return true;
        }

        return false;
    }
}
