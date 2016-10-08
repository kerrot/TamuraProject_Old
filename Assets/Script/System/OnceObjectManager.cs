using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class OnceObjectManager : MonoBehaviour
{
    struct OnceObject
    {
        public string sceneName;
        public int guid;
    };

    static List<OnceObject> onceObjs = new List<OnceObject>();

    static public void RegisterObject(string sceneName, int guid)
    {
        onceObjs.Add(new OnceObject() { sceneName = sceneName, guid = guid });
    }

    static public void ClearRegister()
    {
        onceObjs.Clear();
    }

    void Start()
    {
        int guid = 0;
        string sceneName = SceneManager.GetActiveScene().name;

        OnceInGame[] objs = GameObject.FindObjectsOfType<OnceInGame>();
        objs.ToList().ForEach(o => 
        {
            int tmp = ++guid;
            o.SetGuid(tmp);

            if (onceObjs.Any(oo => oo.sceneName == sceneName && oo.guid == tmp))
            {
                o.gameObject.SetActive(false);
            }
        });
    }
}
