﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class ActDestroy : ActBase
{
    public override void Action(ActionParam param)
    {
        Destory(param.obj, param.param);
    }

    protected void Destory(GameObject obj, float time)
    {
        if (obj != null)
        {
            Collider2D c = obj.GetComponent<Collider2D>();
            if (c != null)
            {
                c.enabled = false;
            }

            if (time > 0)
            {
                GameObject.FindObjectOfType<MonoBehaviour>().StartCoroutine(DestroyWaitTime(obj, time));
            }
            else
            {
                DoDestroy(obj);
            }
        }
    }

    IEnumerator DestroyWaitTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        DoDestroy(obj);
    }

    void DoDestroy(GameObject obj)
    {
        OnceInGame once = obj.GetComponent<OnceInGame>();
        if (once != null)
        {
            once.BeDestroyed();
        }
        GameObject.Destroy(obj);
    }
}
