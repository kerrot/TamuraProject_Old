using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class MushiHead : MusiControl
{
    [SerializeField]
    private PathEditor editor;
    [SerializeField]
    private float speed;
    [SerializeField]
    private List<MushiBody> bodys = new List<MushiBody>();
    [SerializeField]
    private GameObject DieEffect;

    public float Speed { get { return speed; } }

    bool isdead = false;
    GameObject lastDestination;
	BossTraceCheck trace;

    protected override void Awake()
    {
        base.Awake();

		trace = GameObject.FindObjectOfType<BossTraceCheck> ();

        FindNearestDestination();
        transform.LookAt(destination.transform);

        foreach (var b in bodys)
        {
            b.InitDestination(speed);
        }
    }

    void FixedUpdate()
    {
        if (isdead)
        {
            return;
        }

        Vector3 step = (destination.transform.position - transform.position).normalized * speed * Time.deltaTime;
        step.z = 0;
        if (step.magnitude == 0 || Vector3.Distance(destination.transform.position, transform.position) <= step.magnitude)
        {
            transform.position = destination.transform.position;
            FindNextDestination();

            transform.LookAt(destination.transform);
        }
        else
        {
            transform.position += step;
        }
    }

    void FindNearestDestination()
    {
        lastDestination = destination;

        editor.Paths.ForEach(p =>
        {
            if (p.from != null && p.to != null)
            {
                float fromDistance = Vector3.Distance(p.from.transform.position, transform.position);
                float toDistance = Vector3.Distance(p.to.transform.position, transform.position);

                GameObject closer = (fromDistance > toDistance) ? p.to.gameObject : p.from.gameObject;

                if (destination == null ||
                    Vector3.Distance(closer.transform.position, transform.position) < Vector3.Distance(destination.transform.position, transform.position))
                {
                    destination = closer;
                }
            }
        });
    }

    void FindNextDestination()
    {
        PlayerControl player = FindObjectOfType<PlayerControl>();
		if (player != null && trace.PlayerInRange)
        {
            CloseToPlayerDestination(player.transform.position);
        }
        else
        {
            RandomDestination();
        }
    }

    void RandomDestination()
    {
        var paths = editor.Paths.FindAll(p => p.from.gameObject == destination || p.to.gameObject == destination);
        if (paths.Count > 0)
        {
            if (paths.Count > 1)
            {
                while (true)
                {
                    int index = Random.Range(0, paths.Count);
                    if (paths[index].from.gameObject != lastDestination && paths[index].to.gameObject != lastDestination)
                    {
                        lastDestination = destination;
                        destination = (paths[index].from.gameObject == destination) ? paths[index].to.gameObject : paths[index].from.gameObject;
                        return;
                    }
                }
            }
            else
            {
                lastDestination = destination;
                destination = (paths[0].from.gameObject == destination) ? paths[0].to.gameObject : paths[0].from.gameObject;
            }
        }
    }

    void CloseToPlayerDestination(Vector3 playerPosition)
    {
        Vector3 toPlayer = playerPosition - transform.position;

        List<GameObject> tmpList = GetPossibleDestination();
        if (tmpList.Count > 0)
        {
            if (tmpList.Count > 1)
            {
                GameObject last = tmpList.Find(g => g == lastDestination);
                if (last != null)
                {
                    tmpList.Remove(last);
                }

                GameObject nearest = tmpList[0];
                float angle = Vector3.Angle(toPlayer, nearest.transform.position - transform.position);
                for (int i = 1; i < tmpList.Count; ++i)
                {
                    Vector3 toDestination = tmpList[i].transform.position - transform.position;
                    float tmp = Vector3.Angle(toPlayer, toDestination);
                    if (tmp < angle)
                    {
                        angle = tmp;
                        nearest = tmpList[i];
                    }
                }
                lastDestination = destination;
                destination = nearest;
            }
            else
            {
                lastDestination = destination;
                destination = tmpList[0];
            }
        }
    }

    List<GameObject> GetPossibleDestination()
    {
        List<GameObject> tmpList = new List<GameObject>();
        var paths = editor.Paths.FindAll(p => p.from.gameObject == destination || p.to.gameObject == destination);
        paths.ForEach(p =>
        {
            tmpList.Add((p.from.gameObject == destination) ? p.to.gameObject : p.from.gameObject);
        });

        return tmpList;
    }

    override public bool IsHitted(WireControl wire, RaycastHit2D hit)
    {
        if (!isdead)
        {
            Action(wire, hit);

            if (bodys.Count > 0)
            {
                MushiBody b = bodys[bodys.Count - 1];
                bodys.Remove(b);
                DestroyObject(b.gameObject);
            }
            else
            {
                DieEffect.SetActive(true);
                isdead = true;
				GameObject.FindObjectOfType<PlayerControl> ().SetRevivePos (Vector3.zero);
				GetComponent<Collider2D> ().enabled = false;
                StartCoroutine(GameClear());
            }

            return true;
        }

        return false;
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameClear");
    }
}