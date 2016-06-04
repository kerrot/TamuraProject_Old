using UnityEngine;
using System.Collections;

public class OrangeController : MonoBehaviour {
    public void TriggerAnim()
    {
        Animation anim = GetComponent<Animation>();
        if (anim != null)
        {
            anim.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Green")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
