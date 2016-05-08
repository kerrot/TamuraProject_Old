using UnityEngine;
using System.Collections;

public class WireControl : MonoBehaviour {

    public float wireX;
    public float wireY;
    public int offSet;

    public float WireMaxLength;
    public float WireTime;

    public bool IsWiring { get { return line.enabled; } }
    private GameObject Hand;

    private LineRenderer line;
    private int layerMask;

    private bool isReached = false;
    private bool isHit = false;
    private float wireCounter = 0;
    private GameObject player;
    private PlayerControl playerCtrl;
    private Vector3 target;

    // Use this for initialization
    void Start () {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        layerMask = 1 << LayerMask.NameToLayer("Hittable");
        player = transform.parent.gameObject;
        playerCtrl = player.GetComponent<PlayerControl>();
        Hand = transform.FindChild("Hand").gameObject;
    }

    public void ShootWire()
    {
        if (!line.enabled)
        {
            target = ComputeWireTarget();
            line.enabled = true;
            SetHandRotation(target - player.transform.position);
            LineReset();
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
                if (playerCtrl.hitWire == this)
                {
                    DrawWire(target);
                }
                else
                {
                    wireCounter -= Time.deltaTime;
                    float rate = wireCounter / WireTime;
                    if (wireCounter < 0)
                    {
                        line.enabled = false;
                        return;
                    }
                    DrawWire((target - player.transform.position) * rate + player.transform.position);
                }
            }
            else
            {
                wireCounter += Time.deltaTime;
                float rate = wireCounter / WireTime;
                if (wireCounter > WireTime)
                {
                    rate = 1;
                    wireCounter = WireTime;
                    isReached = true;
                    if (isHit)
                    {
                        playerCtrl.SetTarget(target);
                        playerCtrl.hitWire = this;
                    }
                }
                DrawWire((target - player.transform.position) * rate + player.transform.position);
            }
        }
    }

    void LineReset()
    {
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
        wireCounter = 0;
        isReached = false;
    }

    void DrawWire(Vector2 vec)
    {
        Vector2 tmp = player.transform.position;
        line.SetPosition(0, tmp);
        line.SetPosition(1, vec);
        line.material.mainTextureScale = new Vector2((vec - tmp).magnitude, 1);

        SetHandRotation(vec - tmp);
    }

    Vector2 ComputeScreenPosition()
    {
        Vector3 position = Input.mousePosition;

        float l = wireX * ((float)(position.x - Screen.width / 2)) / ((float)(Screen.width / 2));
        float w = wireY * ((float)(position.y - Screen.height / 2 - offSet)) / ((float)(Screen.height / 2));

        return new Vector2(l, w);
    }

    Vector3 ComputeWireTarget()
    {
        Vector2 direction = ComputeScreenPosition();
        Vector2 tmpTarget;
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, WireMaxLength, layerMask);

        if (hit.collider != null)
        {
            isHit = true;
            tmpTarget = hit.point;
        }
        else
        {
            tmpTarget = (Vector2)player.transform.position + direction.normalized * WireMaxLength;
            isHit = false;
        }

        return new Vector3(tmpTarget.x, tmpTarget.y, 0);
    }
}
