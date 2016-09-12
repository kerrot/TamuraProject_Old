using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EndingStage : MonoBehaviour {
    [SerializeField]
    private List<GameObject> List = new List<GameObject>();

    static private int coin;

    static public void AddCoin()
    {
        ++coin;
    }

    static public void ClearCoin()
    {
        coin = 0;
    }

    void Start()
    {
        List.ForEach(l => l.SetActive(List.IndexOf(l) < coin));
    }
}
