using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public void Attacked()
    {
        AttackedReaction();
    }

    protected virtual void AttackedReaction()
    {
        DestroyObject(gameObject);
    }
}
