using UnityEngine;
using System.Collections;

public class GetDrillLocation : MonoBehaviour {

	// Use this for initialization
	//public Vector3 Position;
	public GameObject Drill;
	public GameObject Player;
    public Camera MapCamera;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Player.GetComponent<DropDrill> ().Drill == null) {
            transform.position = new Vector3(MapCamera.transform.position.x + Drill.transform.position.x, Drill.transform.position.y + 2.0f, Drill.transform.position.z);
		}
	
	}
}
