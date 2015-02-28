using UnityEngine;
using System.Collections;

public class CloseWall : MonoBehaviour {

    public GameObject target;

    

	// Use this for initialization
	void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);

       //player position - wall position.sqrMagnitude < dist * dist
        if((this.gameObject.transform.position - target.gameObject.transform.position).sqrMagnitude < distance * distance)
        {
            
        }
        


	}
}
