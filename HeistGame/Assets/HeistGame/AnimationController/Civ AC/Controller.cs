using UnityEngine;
using System.Collections;
using RAIN.Action;
using RAIN.Core;
public class Controller : MonoBehaviour 
{
    public GameObject[] Civilians;

	public Animator anim;
	// Use this for initialization
	void Start () 
	{
        
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("State") == "MOVETOTARGET")
        {
            this.anim.SetBool("Walking", true);
        }

        if (this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<int>("varAfraid") == 100)
        {
            this.anim.SetBool("Walking", false);
            this.anim.SetBool("Coward", true);
            
        }
        if (this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<int>("varAfraid") >= 200)
        {
            this.anim.SetBool("YelledAt", true);
        }

        if (this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("State") != "MOVETOTARGET" && this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varIdle") == "SMOKE")
        {
            this.anim.SetBool("Walking", false);
            this.anim.SetBool("Running", false);
            this.anim.SetInteger("IdleNumber", 1);
        }
        else
        {
            this.anim.SetInteger("IdleNumber", 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.anim.SetBool("Running", true);
        }

	//		anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
	//		anim.SetFloat("Vertical", Input.GetAxis("Vertical"));

	}
}
