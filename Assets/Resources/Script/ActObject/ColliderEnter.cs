using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ColliderEnter : MonoBehaviour
{
    [SerializeField]
    protected List<ColliderActionUnit> actions = new List<ColliderActionUnit>();

    public enum ActionTarget
    {
        SELF,
        TARGET,
    }

    [Serializable]
    public struct ColliderActionUnit
    {
        public float param;
        public ActionTarget target;
        public ActBase.ActionType type;
    }

    protected ActBase.ActionParam param = new ActBase.ActionParam();

    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        Action(coll.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Action(other.gameObject);
    }

    protected void Action(GameObject target)
    {
        param.self = gameObject;
        actions.ForEach(a =>
        {
            ActBase act = ActBase.GetAction(a.type);
            if (act != null)
            {
                param.param = a.param;
                param.obj = (a.target == ActionTarget.SELF) ? gameObject : target;
                act.Action(param);
            }
        });
    }
}