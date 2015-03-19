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
        Civilians = GameObject.FindGameObjectsWithTag("Civilian");

        for (int i = 0; i < Civilians.Length; i++)
        {
            if (Civilians[i].GetComponent<AIRig>().AI.WorkingMemory.GetItem("State").ToString() == "MOVETOTARGET")
            {
                anim.SetBool("Walking", true);
            }
            //else if(Civilians[i].GetComponent<AIRig>().AI.WorkingMemory.GetItem("varIdle").ToString() == "SMOKE")
            //{
            //    anim.SetBool("Smoking", true);
            //}
            else
            {
                anim.SetBool("Smoking", false);
                anim.SetBool("Walking", false);
            }
        }

	//		anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
	//		anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
		/*if(Input.GetKey(KeyCode.W))
		{
		anim.SetBool ("Walking", true);
		}
		else
		{
			anim.SetBool ("Walking", false);
		}

		if(Input.GetKey(KeyCode.S))
		{
			anim.SetBool ("Backward", true);
		}
		else
		{
			anim.SetBool ("Backward", false);
		}*/

	}
}
