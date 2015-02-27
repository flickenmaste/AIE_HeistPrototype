using UnityEngine;
using System.Collections;

public class WallKill : MonoBehaviour {

	public Transform Player;


	void OnTriggerEnter(Collider coll)
	{
		if (coll.attachedRigidbody == Player.rigidbody)
						Application.LoadLevel (Application.loadedLevel);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
}
