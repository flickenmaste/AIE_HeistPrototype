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

	public Vector3 PlayerPosition;

	public void SetPath(int number)
	{
		Cops[number].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varPath", "Path");
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
			//player found, attack the player
			if (Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varPlayer") != null
			    && Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varBreak").Equals(1))
			{
				Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varObjective", "Attack");
			}
			
			if (Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varPlayer") == null
			    && Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varBreak").Equals(1))
			{
				Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varObjective", "Search");
			}
			
			//no player found, follow patrol route
			if (Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varPlayer") == null
			    && Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varBreak").Equals(0))
			{
				SetPath(i);
				Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varObjective", "Patrol");
			}
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}