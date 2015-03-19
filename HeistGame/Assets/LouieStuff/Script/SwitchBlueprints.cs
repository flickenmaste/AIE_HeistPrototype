using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchBlueprints : MonoBehaviour {

	public List<GameObject> Blueprint;
	public int CurrentCameraLook;
	// Use this for initialization
	void Start () {
		CurrentCameraLook = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.UpArrow) && CurrentCameraLook < (Blueprint.Capacity-1)) {
			CurrentCameraLook++;
			transform.position = new Vector3 (transform.position.x, transform.position.y + 6.0f, transform.position.z);
		} else if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.DownArrow) && CurrentCameraLook != 0) {
			CurrentCameraLook--;
			transform.position = new Vector3 (transform.position.x, transform.position.y - 6.0f, transform.position.z);
		}
	}
}
