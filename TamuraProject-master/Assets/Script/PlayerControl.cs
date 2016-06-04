using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;             
    public WireControl hitWire;

    private Rigidbody2D rb2d;       
    private WireControl wire1;
    private WireControl wire2;

    private GameObject PlayerImage;


    void Start()
    {
        
        rb2d = GetComponent<Rigidbody2D>();

        wire1 = transform.GetChild(0).GetComponent<WireControl>();
        wire2 = transform.GetChild(1).GetComponent<WireControl>();
        PlayerImage = transform.FindChild("PlayerImage").gameObject;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (wire1.IsWiring)
            {
                wire2.ShootWire();
            }
            else
            {
                wire1.ShootWire();
            }
        }
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (hitWire != null && hitWire.WireDestination != null &&
            hitWire.WireDestination.transform.parent.gameObject.tag == "Blue")
        {
            Vector2 direction = hitWire.WireDestination.transform.position - transform.position;
            rb2d.MovePosition(rb2d.position + direction.normalized * speed * Time.deltaTime);
            PlayerImage.transform.localScale = new Vector3((direction.x > 0 ? -1 : 1), 1, 1);
        }
    }
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player") {
			Destroy (gameObject);
		}
		if (hit.gameObject.CompareTag ("Player")) {

			// 現在のシーン番号を取得
			int sceneIndex = SceneManager.GetActiveScene().buildIndex;

			// 現在のシーンを再読込する
			SceneManager.LoadScene(sceneIndex);
		}
	}

	/*void onGUI(){
		if (PlayerControl.Count == 0) {
		StartCoroutine("ReloadGame");
		}
	}*/
			
}
