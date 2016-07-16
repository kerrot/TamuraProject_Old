using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class HittableObject : MonoBehaviour
{
    [SerializeField]
    protected List<HittableActionUnit> actions = new List<HittableActionUnit>();

    [Serializable]
    public struct HittableActionUnit
    {
        public float param;
        public ActBase.ActionType type;
    }

    protected ActBase.ActionParam param = new ActBase.ActionParam();

    virtual protected void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Hittable");
    }

    virtual public bool IsHitted(WireControl wire, RaycastHit2D hit)
    {
        Action(wire, hit);

        return true;
    }

    protected void Action(WireControl wire, RaycastHit2D hit)
    {
        param.obj = hit.collider.gameObject;

        actions.ForEach(a =>
        {
            ActBase act = ActBase.GetAction(a.type);
            if (act != null)
            {
                param.param = a.param;
                act.Action(param);
            }
        });
    }
}