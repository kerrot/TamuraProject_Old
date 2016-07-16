using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ColliderCondition : MonoBehaviour
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

    void OnCollisionEnter2D(Collision2D coll)
    {
        Action(coll.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Action(other.gameObject);
    }

    void Action(GameObject target)
    {
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