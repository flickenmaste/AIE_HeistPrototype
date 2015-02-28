using UnityEngine;
using System.Collections;

public class WallRunTrigger : MonoBehaviour {

    [SerializeField]
    public int WallJumpsAllowed = 1;
    
    // Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<PlayerController>().IsWallRunning = true;
            c.gameObject.GetComponent<PlayerController>().JumpCount = WallJumpsAllowed;
        }
    }

    void OnTriggerStay(Collider c)
    {

    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Player")
            c.gameObject.GetComponent<PlayerController>().IsWallRunning = false;
    }
}
