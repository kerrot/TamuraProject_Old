using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class ActAnimation : ActBase
{
    public override void Action(ActionParam param)
    {
        if (param.obj != null)
        {
            Animation anim = param.obj.GetComponent<Animation>();
            if (anim != null)
            {
                anim.Play();
            }
        }
    }
}
