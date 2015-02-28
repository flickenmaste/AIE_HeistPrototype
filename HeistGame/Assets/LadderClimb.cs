using UnityEngine;
using System.Collections;

public class LadderClimb : MonoBehaviour {
	

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag == "Player") 
		{
			coll.rigidbody.useGravity = false;
			coll.gameObject.GetComponent<PlayerController>().gravityMultipler = 0;
		}
	}

	void OnTriggerStay(Collider coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			coll.rigidbody.AddForce (Vector3.up * 10, ForceMode.Force);
		}
	}

	void OnTriggerExit(Collider coll)
	{
		if (coll.gameObject.tag == "Player") 
		{
			coll.rigidbody.useGravity = true;
			coll.gameObject.GetComponent<PlayerController>().gravityMultipler = 1.0f;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
