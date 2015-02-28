using UnityEngine;
using System.Collections;

public class WallKill : MonoBehaviour {

	public Transform Player;

	public float SpeedX;

	public float SpeedY;

	public float SpeedZ;

	void OnTriggerEnter(Collider coll)
	{
		if (coll.attachedRigidbody == Player.rigidbody)
						Application.LoadLevel (Application.loadedLevel);
	}

	// Use this for initialization
	void Start () {
		this.transform.rigidbody.AddForce(SpeedX, SpeedY, SpeedZ);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
