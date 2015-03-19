using UnityEngine;
using System.Collections;

public class PingManager : MonoBehaviour {

	// Use this for initialization
	public GameObject Manager;
    public GameObject Blueprint;
	void Start () {
	
	}


	// Update is called once per frame
	void Update () {
        if (Manager.GetComponent<ImportantPeaple>().Found == true)
            transform.position = Manager.transform.position + Blueprint.transform.position;
	}
}
