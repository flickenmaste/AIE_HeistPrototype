using UnityEngine;
using System.Collections;

public class DropDrill : MonoBehaviour {

	// Use this for initialization
	public GameObject Drill;
	public GetDrillLocation ObjectDrill;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F) && Drill != null){
			ObjectDrill.Drill =  GameObject.Instantiate (Drill, new Vector3(transform.position.x,transform.position.y,transform.position.z),new Quaternion(0,0,0,0)) as GameObject;
			Drill = null;
		}
	}
}
