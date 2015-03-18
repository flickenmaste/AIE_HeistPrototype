using UnityEngine;
using System.Collections;

public class PingManager : MonoBehaviour {

	// Use this for initialization
	public GameObject Manager;
	void Start () {
	
	}


	// Update is called once per frame
	void Update () {
		if (Manager.GetComponent<ImportantPeaple> ().Found == true)
			transform.position = new Vector3 (Manager.transform.position.x + 200.0f, Manager.transform.position.y, Manager.transform.position.z);
	}
}
