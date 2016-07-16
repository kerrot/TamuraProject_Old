using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class ActPlayerDie : ActBase
{
    public override void Action(ActionParam param)
    {
        PlayerControl player = GameObject.FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            player.Die();
        }
    }
}
