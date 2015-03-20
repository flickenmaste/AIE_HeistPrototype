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
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////fix these later
        if (this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varState") == "MOVETOTARGET")
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

        if (this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varState") != "MOVETOTARGET" && this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varIdle") == "SMOKE")
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

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetState(GameObject ai)
    {
        string State = ai.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varState");
        return State;
    }

    public void SetState(GameObject ai, string state)
    {
        ai.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varState", state);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public int GetAfraid(GameObject ai)
    {
        int Afraid = ai.GetComponent<AIRig>().AI.WorkingMemory.GetItem<int>("varAfraid");
        return Afraid;
    }

    public void SetAfraid(GameObject ai, int afraid)
    {
        ai.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varAfraid", afraid);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public string GetPath(GameObject ai)
    {
        string Path = ai.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varPath");
        return Path;
    }

    public void SetPath(GameObject ai, string path)
    {
        ai.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varState", path);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public string GetGoal(GameObject ai)
    {
        string Goal = ai.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varGoal");
        return Goal;
    }

    public void SetGoal(GameObject ai, string goal)
    {
        ai.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varGoal", goal);
    }
}
