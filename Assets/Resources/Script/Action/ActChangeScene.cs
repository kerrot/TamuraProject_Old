using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ActChangeScene : ActBase
{
    private string[] scene = { "title", "stage", "kerrot", "masuta-sute-zi" };

    public override void Action(ActionParam param)
    {
        SceneManager.LoadScene(scene[(int)param.param]);
    }
}
