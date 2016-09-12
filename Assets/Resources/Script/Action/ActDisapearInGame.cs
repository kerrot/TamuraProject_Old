using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ActDisapearInGame : ActBase
{
    public override void Action(ActionParam param)
    {
        param.self.SetActive(false);
    }
}
