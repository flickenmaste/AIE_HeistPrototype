using UnityEngine;
using System.Collections;

public class Toggle_Map : MonoBehaviour {

	public Camera MapCamera;
	public bool Active;
	public MouseLook look;
	// Use this for initialization
	void Start () {
		Active = false;
		ShowMap ();
	
	}
	void ShowMap()
	{
		if (MapCamera.GetComponent<TakeScreenShot> ().Captured == false) {
			if (Active == false) {

				MapCamera.transform.gameObject.SetActive (false);
				look.enabled = true;

			} else if (Active == true) {
				MapCamera.transform.gameObject.SetActive (true);
				look.enabled = false;
			}
		} else
			look.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M)) {
			Active = !Active;
		}
		ShowMap();

	}
}
