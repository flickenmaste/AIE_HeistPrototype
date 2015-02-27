using UnityEngine;
using System.Collections;

public class WallRunTrigger : MonoBehaviour {

    
    // Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnTriggerStay(Collider c)
    {
        c.gameObject.GetComponent<PlayerController>().IsWallRunning = true;
    }

    void OnTriggerExit(Collider c)
    {
        c.gameObject.GetComponent<PlayerController>().IsWallRunning = false;
    }
}
