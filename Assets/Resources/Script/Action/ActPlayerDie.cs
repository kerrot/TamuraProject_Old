using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class ActPlayerDie : ActBase
{
    public override void Action(ActionParam param)
    {
        if (param.obj != null)
        {
            PlayerControl player = param.obj.GetComponent<PlayerControl>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
