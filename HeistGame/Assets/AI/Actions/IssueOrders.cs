using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SetOrders : RAINAction
{
	public GameObject[] Cops;
	public GameObject Commander;

	public void SetPath(int number)
	{
		Cops[number].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varPath", "Path");
	}

	//function to see if the cops have reached the previous player location after they have lost sight
	void SearchAround()
	{

	}

    public override void Start(RAIN.Core.AI ai)
    {
		Cops = GameObject.FindGameObjectsWithTag("Cop");
		Commander = GameObject.FindGameObjectWithTag("Commisioner");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		for (int i = 0; i < Cops.Length; i++)
		{
			if (Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varObjective").Equals("Attack"))
			{

			}

			if (Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varObjective").Equals("Patrol"))
			{
				SetPath(i);
			}
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}