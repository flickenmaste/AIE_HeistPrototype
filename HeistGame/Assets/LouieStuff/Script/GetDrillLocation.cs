using UnityEngine;
using System.Collections;

public class GetDrillLocation : MonoBehaviour {

	// Use this for initialization
	//public Vector3 Position;
	public GameObject Drill;
	public GameObject Player;
    public GameObject Blueprint;
    public GameObject Bank;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Player.GetComponent<DropDrill> ().Drill == null) {
            transform.position = new Vector3( Drill.transform.position.x, Drill.transform.position.y, Drill.transform.position.z);
		}
	
	}
}
