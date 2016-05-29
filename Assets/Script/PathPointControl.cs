using UnityEngine;
using System;
using System.Collections;

public class PathPointControl : MonoBehaviour {

    public event Action<GameObject> OnClick;
    public event Action<GameObject> OnDelete;
    public event Action<GameObject> OnCreate;

    void Awake()
    {
        if (transform.parent != null)
        {
            PathEditor editor = transform.parent.gameObject.GetComponent<PathEditor>();
            if (editor != null)
            {
                if (OnCreate == null)
                {
                    editor.RegisterPoint(this);
                }

                OnCreate(gameObject);
            }
        }
    }

    void OnMouseUp()
    {
        if (transform.parent.gameObject.GetComponent<PathEditor>() != null)
        {
            OnClick(gameObject);
        }
    }

    void OnDestroy()
    {
        if (transform.parent.gameObject.GetComponent<PathEditor>() != null)
        {
            OnDelete(gameObject);
        }
    }
}
