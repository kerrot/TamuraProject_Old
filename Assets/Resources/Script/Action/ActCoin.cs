using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ActCoin : ActBase
{
    public override void Action(ActionParam param)
    {
        EndingStage.AddCoin();
    }
}
