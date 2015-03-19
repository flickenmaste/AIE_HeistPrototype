using UnityEngine;
using System.Collections;

public class MouseWheelrotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			transform.Rotate(Vector3.up * 5.0f, Space.Self);
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			transform.Rotate(Vector3.down * 5.0f, Space.Self);
		}
	}
}
