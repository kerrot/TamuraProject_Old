using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class ActDestroyGreen : ActDestroy
{
    public override void Action(ActionParam param)
    {
        if (param.obj != null && param.obj.tag == "Green")
        {
            Destory(param.obj, param.param);
        }
    }
}
