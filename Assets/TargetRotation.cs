using UnityEngine;
using System.Collections;

public class TargetRotation : MonoBehaviour {
	[SerializeField]
	GameObject parentObject;        // 親オブジェクト

	// Use this for initialization
	void Start () {
		// 親のオブジェクトを取得する。
		parentObject = GameObject.Find("Sample").gameObject;
	}

	// Update is called once per frame
	void Update () {
		// Z軸を自オブジェクトの回転を使って回転する。
		Vector3 forward = transform.rotation * Vector3.forward;
		// 念のために正規化する。
		forward.Normalize();
		// Y軸を自オブジェクトの回転を使って回転する。
		Vector3 up = transform.rotation * Vector3.up;
		// 念のために正規化する。
		up.Normalize();

		Quaternion q;
		if (Input.GetKey(KeyCode.UpArrow)) {
			// 上キーの場合は、ローカルのZ軸で回転する。
			q = Quaternion.AngleAxis(1, forward);
		}
		else if (Input.GetKey(KeyCode.DownArrow)) {
			// 下キーの場合は、ローカルのZ軸で回転する。
			q = Quaternion.AngleAxis(-1, forward);
		}
		else if (Input.GetKey(KeyCode.RightArrow)) {
			// 右キーの場合は、ローカルのY軸で回転する。
			q = Quaternion.AngleAxis(1, up);
		}
		else if (Input.GetKey(KeyCode.LeftArrow)) {
			// 左キーの場合は、ローカルのY軸で回転する。
			q = Quaternion.AngleAxis(-1, up);
		}
		else {
			// 入力がない場合は何もしない。
			return;
		}
		// 自オブジェクトを回転する。
		transform.rotation = q * transform.rotation;

		// 親オブジェクトの位置を取得する。
		var parentPsosition = parentObject.transform.position;
		// 親オブジェクトから自オブジェクトへ向かうベクトルを作成する。
		var dir = transform.position - parentPsosition;
		// 自オブジェクトの位置を計算するために、原点でベクトルを回転後、親オブジェクトの座標で移動させる。
		transform.position = q * dir + parentPsosition;
	}
}
